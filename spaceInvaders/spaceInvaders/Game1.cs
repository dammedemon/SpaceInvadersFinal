using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace spaceInvaders;

public class Game1 : Game
{
    Player player;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D playerTex = Content.Load<Texture2D>("Ship_01-1");
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height - 20);
        int stopX = Window.ClientBounds.Width - playerTex.Width;

        player = new Player(playerTex, playerPos, stopX);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        player.Update();

        base.Update(gameTime);
    }

     protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(player.playerTex, player.playerPos, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
