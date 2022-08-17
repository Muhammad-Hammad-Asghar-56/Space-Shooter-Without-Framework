using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EZInput;
namespace spaceShooter
{
    public partial class Form1 : Form
    {
        PictureBox spaceShip;
        List<PictureBox> playersFires = new List<PictureBox>();

        List<PictureBox> enemies = new List<PictureBox>();
        List<PictureBox> enimesFires = new List<PictureBox>();
        int SpaceShiphealth = 100;
        int enemeyHealth1 = 100;
        int enemyHealth2 = 100;
        float number;
        float gameSpeed = 0.40F;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateSpcaeship();
            createEnemy(20, 10);
            createEnemy(50, 500);
        }
        public void CreateSpcaeship()
        {
            spaceShip = new PictureBox();
            Image img = spaceShooter.Properties.Resources.spaceShip;
            spaceShip.Image = img;
            spaceShip.BackColor = Color.Transparent;
            spaceShip.Width = img.Width;
            spaceShip.Height = img.Height;
            spaceShip.Left = this.Width / 2 - (spaceShip.Width / 2);
            spaceShip.Top = this.Height - (spaceShip.Height + 10);
            this.Controls.Add(spaceShip);
        }
        //----------------------------- Player Movement-----------------------------------------------------
        public void movePlayer(PictureBox spaceShip)
        {
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                // left move
                movePictureleft(spaceShip, 20);
            }
            if (Keyboard.IsKeyPressed(Key.RightArrow))
            {
                // Right move
                movePictureRight(spaceShip, 20);
            }
            if (Keyboard.IsKeyPressed(Key.UpArrow))
            {
                // Up move
                movePictureUp(spaceShip, 10);
            }
            if (Keyboard.IsKeyPressed(Key.DownArrow))
            {
                // Down move
                movePictureDown(spaceShip, 10);
            }
            if (Keyboard.IsKeyPressed(Key.Space))
            {
                // Fire
                fireFromTop(spaceShip);
            }
        }
        public void movePictureleft(PictureBox player, int movement)
        {
            if (player.Left > 1)
            {
                player.Left = player.Left - movement;
            }
        }
        public void movePictureRight(PictureBox player, int movement)
        {
            if ((player.Left + player.Width) < this.Width)
            {
                player.Left = player.Left + movement;
            }
        }
        public void movePictureUp(PictureBox player, int movement)
        {
            if (player.Top > 1)
            {
                player.Top = player.Top - movement;
            }
        }
        public void movePictureDown(PictureBox player, int movement)
        {
            if ((player.Height + player.Top) < this.Height)
            {
                player.Top = player.Top + movement;
            }
        }
        public void fireFromTop(PictureBox player)
        {
            if (player.Top > 10)
            {
                PictureBox fire = new PictureBox();
                Image img = spaceShooter.Properties.Resources.laserBlueUse;
                fire.Image = img;
                fire.BackColor = Color.Transparent;
                fire.Top = player.Top;
                fire.Left = player.Left + player.Width / 2;
                playersFires.Add(fire);
                this.Controls.Add(fire);
            }
        }
        public void fireFromBottom(PictureBox player)
        {

            PictureBox fire = new PictureBox();
            Image img = spaceShooter.Properties.Resources.laserUse;
            fire.Image = img;
            fire.BackColor = Color.Transparent;
            fire.Top = player.Bottom + img.Height;
            fire.Left = player.Left + player.Width / 2;
            enimesFires.Add(fire);
            this.Controls.Add(fire);
        }
        public void alwaysMoveFireUp(List<PictureBox> fireList)
        {
            for (int i = 0; i < fireList.Count; i++)
            {
                if (fireList[i].Top < 20)
                {
                    fireList[i].Hide();
                    fireList.Remove(fireList[i]);
                }
                else
                {
                    fireList[i].Top -= 20;
                }
            }
        }
        public void alwaysMoveFireDown(List<PictureBox> fireList)
        {
            for (int i = 0; i < fireList.Count; i++)
            {
                if (fireList[i].Top > (this.Height + 20))
                {
                    fireList[i].Hide();
                    fireList.Remove(fireList[i]);
                }
                else
                {
                    fireList[i].Top += 20;
                }
            }
        }
        //--------------------------------------------------------------------------------------
        //--------------------------------------- Enemy-----------------------------------------
        public void createEnemy(int Top, int Left)
        {
            PictureBox enemy = new PictureBox();
            Image img = spaceShooter.Properties.Resources.enemyUser2;
            enemy.Image = img;
            enemy.BackColor = Color.Transparent;
            enemy.Top = Top;
            enemy.Left = Left;
            //enemy.Location=new System.Drawing.Point(Top, Left);
            enemies.Add(enemy);
            this.Controls.Add(enemy);
        }
        public void moveRandom(List<PictureBox> pictureList)
        {
            Random rand = new Random();
            int no;
            if (Math.Floor(number) == 1)
            {
                foreach (var i in pictureList)
                {
                    no = rand.Next();
                    no = no % 5;
                    if (no == 0)
                    {
                        movePictureleft(i, 20);
                    }
                    if (no == 1)
                    {
                        movePictureRight(i, 20);
                    }
                    if (no == 2)
                    {
                        fireFromBottom(i);
                    }
                    if (no == 3)
                    {
                        movePictureDown(i, 15);
                    }
                    if (no == 4)
                    {
                        movePictureUp(i, 15);
                    }
                    number = 0;
                }
            }
        }
        //--------------------------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            movePlayer(spaceShip);
            alwaysMoveFireUp(playersFires);
            moveRandom(enemies);
            alwaysMoveFireDown(enimesFires);
            CrossfiringCancelation(playersFires, enimesFires);
            Healthing();
            number += gameSpeed;
        }
        public bool isCollideWithAnyPicture(List<PictureBox> pictureList, PictureBox picture)
        {
            for (int i = 0; i < pictureList.Count; i++)
            {
                if (pictureList[i].Bounds.IntersectsWith(picture.Bounds))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsCollideWithAnyPicture(List<PictureBox> pictureList)
        {
            for (int i = 0; i < pictureList.Count; i++)
            {
                for (int j = 0; j < pictureList.Count; j++)
                {
                    if (pictureList[i].Bounds.IntersectsWith(pictureList[j].Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void CrossfiringCancelation(List<PictureBox> domainentFire, List<PictureBox> weekFire)
        {
            for (int i = 0; i < weekFire.Count; i++)
            {
                if (isCollideWithAnyPicture(domainentFire, weekFire[i]))
                {
                    weekFire[i].Hide();
                    weekFire.Remove(weekFire[i]);
                }
            }
        }
        /******
         * ************
         * **************** healthing
         * ************
         * ****
         */
        public void Healthing()
        {
            playerHealthing(SpaceShiphealth, spaceShip, enimesFires);
            EnemyHealthing(enemies, playersFires);
            if (SpaceShiphealth < 10)
            {
                spaceShip.Hide();
            }
            if (enemies.Count > 0)
            {
                if (enemeyHealth1 < 5)
                {
                    enemies[0].Hide();
                    enemies.RemoveAt(0);
                }
            }
            if (enemies.Count > 1)
            {
                if (enemyHealth2 < 5)
                {
                    enemies[1].Hide();
                    enemies.RemoveAt(1);
                }
            }
        }
        public void playerHealthing(int Health, PictureBox player, List<PictureBox> ReduceByPictureList)
        {
            for (int i = 0; i < ReduceByPictureList.Count; i++)
            {
                if (player.Bounds.IntersectsWith(ReduceByPictureList[i].Bounds))
                {
                    Health -= 50;
                    ReduceByPictureList[i].Hide();
                    ReduceByPictureList.Remove(ReduceByPictureList[i]);
                }
            }
        }
        public void EnemyHealthing(List<PictureBox> enemniesList, List<PictureBox> ReduceByPictureList)
        {
            for (int i = 0; i < enemniesList.Count; i++)
            {
                for (int j = 0; j < ReduceByPictureList.Count; j++)
                {
                    if (enemies.Count > 0 && ReduceByPictureList.Count > 0)
                    {
                        bool status = enemniesList[i].Bounds.IntersectsWith(ReduceByPictureList[j].Bounds);
                        if (status)
                        {
                            if (i == 0)
                            {
                                enemeyHealth1 -= 20;
                            }
                            if (i == 1)
                            {
                                enemyHealth2 -= 40;
                            }
                            ReduceByPictureList[j].Hide();
                            ReduceByPictureList.Remove(ReduceByPictureList[j]);
                        }
                    }
                }
            }
        }
    }
}
