using JetBoxer2D.Engine.Events;
using JetBoxer2D.Engine.Extensions;
using JetBoxer2D.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Engine;

public class MainGame : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private BaseGameState currentGameState;

    //Window Scaling
    private RenderTarget2D renderTarget; //will hold the desired resolution target
    private Rectangle renderScaleRectangle; //will hold the scale rectangle
    
    private int _initialResolutionWidth;
    private int _initialResolutionHeight;
    private float _initialResolutionAspectRatio;
    private BaseGameState _initialGameState;

    public MainGame(int width, int height, BaseGameState initialGameState)
    {
        Content.RootDirectory = "Content";
        graphics = new GraphicsDeviceManager(this);
        
        _initialResolutionWidth = width;
        _initialResolutionHeight = height;
        _initialResolutionAspectRatio = width / (float) height;
        
        _initialGameState = initialGameState;

        //Responsible for initializing and updating many singleton classes such as Time and MouseInput
        SingletonManager.InitializeSingletons();
    }

    protected override void Initialize()
    {
        //Window Scaling
        graphics.PreferredBackBufferWidth = _initialResolutionWidth;
        graphics.PreferredBackBufferHeight = _initialResolutionHeight;
        graphics.IsFullScreen = false;
        graphics.ApplyChanges();
        
        renderTarget = new RenderTarget2D(graphics.GraphicsDevice,
            _initialResolutionWidth, _initialResolutionHeight,
            false, SurfaceFormat.Color, DepthFormat.None, 0,
            RenderTargetUsage.DiscardContents);

        renderScaleRectangle = GetScaleRectangle();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        SwitchState(_initialGameState);
    }

    protected override void UnloadContent()
    {
        currentGameState?.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        SingletonManager.UpdateSingletons(gameTime);
        currentGameState?.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        //Render the current GameState's gameObjects
        spriteBatch.Begin();
        currentGameState.Render(spriteBatch);
        spriteBatch.End();

        //Render the scaled window content
        graphics.GraphicsDevice.SetRenderTarget(null);
        graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        spriteBatch.Draw(renderTarget, renderScaleRectangle, Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }

    //Provides black bars akin to the black scalers of TV screen based on actual resolution
    private Rectangle GetScaleRectangle()
    {
        var variance = 0.5;
        var actualAspectRatio = Window.ClientBounds.Width / (float) Window.ClientBounds.Height;

        Rectangle scaleRectangle;

        if (actualAspectRatio <= _initialResolutionAspectRatio)
        {
            var presentHeight = (int) (Window.ClientBounds.Width / _initialResolutionAspectRatio + variance);
            var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

            scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
        }
        else
        {
            var presentWidth = (int) (Window.ClientBounds.Height * _initialResolutionAspectRatio + variance);
            var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

            scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
        }

        return scaleRectangle;
    }

    //When the current gameState is switched, whatever needs to be done in that time should be placed here
    private void CurrentGameState_OnStateSwitched(object sender, BaseGameState newState)
    {
        SwitchState(newState); //call the SwitchState method from here, so that this method is
        //now tied to the new game state, and not the previous one 
    }

    private void CurrentGameState_OnEventNotification(object sender, BaseGameStateEvent eventType)
    {
        switch (eventType)
        {
            case BaseGameStateEvent.GameQuit:
                Exit();
                break;
        }
    }

    //Change the currentGameState and assign it new event subscriptions, and unsubscribe them from the old
    private void SwitchState(BaseGameState gameState)
    {
        if (currentGameState != null)
        {
            currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
            currentGameState.OnEventNotification -= CurrentGameState_OnEventNotification;
            currentGameState.UnloadContent();
        }

        currentGameState = gameState;

        currentGameState.Initialize(this, Content, graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height); //Setup up the ContentManager of the currentGameState
        currentGameState.LoadContent(spriteBatch);

        currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        currentGameState.OnEventNotification += CurrentGameState_OnEventNotification;
    }
}