using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.GameStates;
using Super_Duper_Shooter.Classes.GameStates.Base;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter;

public class MainGame : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private BaseGameState currentGameState;

    //Window Scaling
    private RenderTarget2D renderTarget; //will hold the desired resolution target
    private Rectangle renderScaleRectangle; //will hold the scale rectangle
    private const int DESIGNED_RESOLUTION_WIDTH = 1280;
    private const int DESIGNED_RESOLUTION_HEIGHT = 720;

    private const float DESIGNED_RESOLUTION_ASPECT_RATIO =
        DESIGNED_RESOLUTION_WIDTH / (float) DESIGNED_RESOLUTION_HEIGHT;

    public MainGame()
    {
        graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        //Responsible for initializing and updating many singleton classes such as Time and MouseInput
        SingletonManager.InitializeSingletons();
    }

    protected override void Initialize()
    {
        //Window Scaling
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.IsFullScreen = false; //Full Screen Mode
        graphics.ApplyChanges();
        renderTarget = new RenderTarget2D(graphics.GraphicsDevice,
            DESIGNED_RESOLUTION_WIDTH, DESIGNED_RESOLUTION_HEIGHT,
            false, SurfaceFormat.Color, DepthFormat.None, 0,
            RenderTargetUsage.DiscardContents);

        renderScaleRectangle = GetScaleRectangle();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        //The first time the game loads, set the current state to the SplashState (aka Main Menu)
        SwitchState(new SplashState());
    }

    protected override void UnloadContent()
    {
        currentGameState?.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        SingletonManager.UpdateSingletons(gameTime);

        currentGameState?.HandleInput(gameTime);

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

        if (actualAspectRatio <= DESIGNED_RESOLUTION_ASPECT_RATIO)
        {
            var presentHeight = (int) (Window.ClientBounds.Width / DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
            var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

            scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
        }
        else
        {
            var presentWidth = (int) (Window.ClientBounds.Height * DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
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

    private void CurrentGameState_OnEventNotification(object sender, GameEvents eventType)
    {
        switch (eventType)
        {
            case GameEvents.GameQuit:
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

        currentGameState.Initialize(Content, graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height); //Setup up the ContentManager of the currentGameState
        currentGameState.LoadContent(spriteBatch);

        currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        currentGameState.OnEventNotification += CurrentGameState_OnEventNotification;
    }
}