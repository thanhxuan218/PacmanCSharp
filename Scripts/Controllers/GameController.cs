using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;
using PacmanWindowsForm.Script.Models;

namespace PacmanWindowForms.Script.Controllers
{
    public class PacmanGame
    {
        public int PacmanSpeed
        {
            get { return _delay; }
            set
            {

                _delay = (value < 10) ? 10 : value;
                Pacman.speed = _delay;
            }
        }

        public int RedGhostSpeed
        {
            get { return RedGhost.speed; }
            set { RedGhost.speed = value; }
        }

        public int BlueGhostSpeed
        {
            get { return BlueGhost.speed; }
            set { BlueGhost.speed = value; }
        }

        public int PinkGhostSpeed
        {
            get { return PinkGhost.speed; }
            set { PinkGhost.speed = value; }
        }

        public int YellowGhostSpeed
        {
            get { return YellowGhost.speed; }
            set { YellowGhost.speed = value; }
        }

        public int GhostSpeed
        {
            get { return _ghostSpeed; }
            set
            {
                _ghostSpeed = (value < 10) ? 10 : value;
                RedGhost.speed = _ghostSpeed;
                BlueGhost.speed = _ghostSpeed;
                PinkGhost.speed = _ghostSpeed;
                YellowGhost.speed = _ghostSpeed;
            }
        }


        private GameState _state = GameState.GameOver;
        public GameState State
        {
            get { return _state; }
            set
            {
                _state = value;
                Pacman.gameState = value;
                RedGhost.gameState = value;
                BlueGhost.gameState = value;
                YellowGhost.gameState = value;
                PinkGhost.gameState = value;
            }
        }

        public Task Runner;
        public Task wallRunner;
        public Task bonusMonitorTask;
        private int _ghostSpeed = 90;
        private int _delay = 90;
        private int score = 0;
        private bool _bonus = false;
        private int Level = 1;
        private int lives = 3;

        public bool Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                _bonus = value;
            }
        }

        private readonly frmPacmanGame parentForm;
        private readonly PacmanBoard board;
        private readonly PacmanCharacter Pacman;
        private StaticEntity staticEntity;
        private Dictionary<GhostColor, GhostCharacter> Ghosts = new Dictionary<GhostColor, GhostCharacter>() {
            { GhostColor.Blue, null },
            { GhostColor.Pink, null },
            { GhostColor.Red, null },
            { GhostColor.Yellow, null }
        };

        List<Point> wallList = new List<Point>();
        List<Point> dotList = new List<Point>();
        List<Point> boxList = new List<Point>();
        List<Point> boxDoorList = new List<Point>();
        List<Point> bonusList = new List<Point>();


        public PacmanGame(frmPacmanGame frm, Panel p)
        {
            parentForm = frm;
            board = new PacmanBoard(p);
            Pacman = new PacmanCharacter(parentForm, board);
            Ghosts[GhostColor.Red] = new GhostCharacter(parentForm, board, GhostColor.Red);
            Ghosts[GhostColor.Blue] = new GhostCharacter(parentForm, board, GhostColor.Blue);
            Ghosts[GhostColor.Pink] = new GhostCharacter(parentForm, board, GhostColor.Pink);
            Ghosts[GhostColor.Yellow] = new GhostCharacter(parentForm, board, GhostColor.Yellow);
            staticEntity = new StaticEntity(board);
            this.Initialize();
            RePaint();
        }


        private void Initialize()
        {
            GameBoard.Instance.LoadBoard();
            wallList = GameBoard.Instance.WallList();
            dotList = GameBoard.Instance.DotList();
            boxList = GameBoard.Instance.BoxList();
            boxDoorList = GameBoard.Instance.BoxDoorList();
            bonusList = GameBoard.Instance.BonusList();

            Pacman.Initialize();
            Ghosts[GhostColor.Red].Initialize();
            Ghosts[GhostColor.Blue].Initialize();
            Ghosts[GhostColor.Pink].Initialize();
            Ghosts[GhostColor.Yellow].Initialize();
            staticEntity.Initialize();

            State = GameState.GameOver;
            score = 0;
            PacmanSpeed = 80;
            GhostSpeed = 80;

            PointLists.PrintMapToFile();
        }


        public void Run()
        {
            RePaint();
            State = GameState.GameRun;
            Runner = new Task(runGame);
            Runner.Start();
            wallRunner = new Task(runWalls);
            wallRunner.Start();
            Clock = new Task(RunClock);
            Clock.Start();
            Pacman.Run();
            Ghosts[GhostColor.Red].Run();
            Ghosts[GhostColor.Blue].Run();
            Ghosts[GhostColor.Pink].Run();
            Ghosts[GhostColor.Yellow].Run();
            staticEntity.StaticEntityStart();
        }

        public void RePaint()
        {
            parentForm.SuspendLayout();
            board.Resize();
            parentForm.ResumeLayout(false);
        }

        private void runGame()
        {
            while (State != GameState.GameOver)
            {
                try
                {
                    eatDots(Pacman.GetCore());
                    EatBonus(Pacman.GetCore());
                    AddLives();
                    BonusClear();
                    BonusPaint(bonusStateCounter);
                    DotPaint();
                    CheckForWin();
                    CheckCollision(this.Pacman);
                    parentForm.Write(score.ToString(), Level.ToString(), ConvertLives(lives), Pacman.GetPoint().ToString(), PacmanSpeed.ToString(), GhostSpeed.ToString());
                    Runner.Wait(10);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private string ConvertLives(int l)
        {
            string lives = string.Empty;
            for (int i = 0; i < l; i++) lives += "❤";
            return lives;

        }

        private void AddLives()
        {
            if (score % 10000 == 0)
            {
                parentForm.playSound(Properties.Resources.Pacman_Extra_Live);
                score += 10;
                lives++;
            }
        }

        private void runWalls()
        {
            while (State != GameState.GameOver)
            {
                try
                {
                    DoorPaint();
                    WallPaint();
                    wallRunner.Wait(100);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private void RunClock()
        {
            while (State != GameState.GameOver)
            {
                try
                {
                    ChangeGhostState();
                    BonusStateChange();
                    Clock.Wait(100);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        // Use timer instead of this
        private void MonitorBonusBehavior()
        {
            bonusMonitorTask = new Task(() =>
            {
            });
            bonusMonitorTask.Start();
        }

        private Dictionary<GhostColor, Timer> ghostStateTimers = new Dictionary<GhostColor, Timer>()
        {
            { GhostColor.Blue, null },
            { GhostColor.Pink, null },
            { GhostColor.Red, null },
            { GhostColor.Yellow, null }
        };

        private void Initialize()
        {
            // Initialize the ghost state timers
            ghostStateTimers[GhostColor.Blue] = new Timer(50);
            ghostStateTimers[GhostColor.Blue].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Blue].AutoReset = true;

            ghostStateTimers[GhostColor.Pink] = new Timer(50);
            ghostStateTimers[GhostColor.Pink].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Pink].AutoReset = true;

            ghostStateTimers[GhostColor.Red] = new Timer(50);
            ghostStateTimers[GhostColor.Red].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Red].AutoReset = true;

            ghostStateTimers[GhostColor.Yellow] = new Timer(50);
            ghostStateTimers[GhostColor.Yellow].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Yellow].AutoReset = true;
        }

        private void GhostStateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Check which ghost caused the event
            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                if (ghostStateTimers[color] == sender)
                {
                    // Check if the ghost is in bonus state
                    if (Ghosts[color].ghostState == GhostState.Bonus)
                    {
                        // Change the ghost state to normal
                        SetGhostState(color, GhostState.Normal);
                    }
                    else if (Ghosts[color].ghostState == GhostState.BonusEnd)
                    {
                        // Change the ghost state to bonus
                        SetGhostState(color, GhostState.Bonus);
                    }
                }
            }
        }

        private void ChangeGhostState()
        {
            // Start the ghost state timer
            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                if (Ghosts[color].ghostState == GhostState.Eaten && this.State == GameState.GameRun)
                {
                    ghostStateTimers[color].Start();
                }
                else
                {
                    ghostStateTimers[color].Stop();
                }
            }
        }

        private void BonusStateChange()
        {

        }



        private void BonusStateChange()
        {
            bonusStateCounter = (bonusStateCounter > 4) ? 1 : bonusStateCounter;
            bonusStateCounter++;
            if (Bonus && bonusCounter < 70 && this.State == GameState.GameRun)
            {
                bonusCounter++;

            }
            else if (Bonus && bonusCounter >= 70 && bonusCounter < 100)
            {
                if (BonusEndSprite)
                {
                    foreach (GhostColor color in ghostStateTimers.Keys)
                    {
                        if (Ghosts[color].ghostState != GhostState.Eaten)
                        {
                            SetGhostState(color, GhostState.BonusEnd);
                        }
                    }
                    BonusEndSprite = false;
                }
                else
                {
                    foreach (GhostColor color in ghostStateTimers.Keys)
                    {
                        if (Ghosts[color].ghostState != GhostState.Eaten)
                        {
                            SetGhostState(color, GhostState.Bonus);
                        }
                    }
                    BonusEndSprite = true;
                }
                bonusCounter++;
            }
            else if (Bonus && bonusCounter >= 100)
            {
                bonusCounter = 0;
                Bonus = false;
                foreach (GhostColor color in ghostStateTimers.Keys)
                {
                    if (Ghosts[color].ghostState != GhostState.Eaten)
                    {
                        SetGhostState(color, GhostState.Normal);
                    }
                }
            }
        }
        private void EatDots(Point[] core)
        {

            for (int i = 0; i <= dotList.Count - 1; i++)
            {
                foreach (Point corePoint in core)
                {
                    if (corePoint.X == dotList[i].X && corePoint.Y == dotList[i].Y)
                    {
                        dotList.RemoveAt(i);
                        score += 20 * Level / 2;
                        break;
                    }
                }
            }
        }

        private void EatBonus(Point[] core)
        {
            for (int i = 0; i <= bonusList.Count - 1; i++)
            {
                foreach (Point corePoint in core)
                {
                    if (corePoint.X == bonusList[i].X && corePoint.Y == bonusList[i].Y)
                    {
                        bonusList.RemoveAt(i);
                        score += 100 * Level / 2;
                        SetGhostState(GhostColor.Blue, GhostState.Bonus);
                        SetGhostState(GhostColor.Red, GhostState.Bonus);
                        SetGhostState(GhostColor.Pink, GhostState.Bonus);
                        SetGhostState(GhostColor.Yellow, GhostState.Bonus);
                        Bonus = true; bonusCounter = 0;
                        break;
                    }
                }
            }
        }

        int eatenScore = 200;

        private void CheckForLose()
        {
            List<Point> mergedList = new List<Point>();
            mergedList = RedGhost.GetCore().Union(BlueGhost.GetCore()).Union(PinkGhost.GetCore()).Union(YellowGhost.GetCore()).ToList();

            List<Point> commonPoints = Pacman.GetCore().Intersect(mergedList.Select(u => u)).ToList();

            if ((commonPoints.Count != 0 && !Bonus) || lives == 0)
            {
                State = GameState.GamePause;
                parentForm.playSound(Properties.Resources.Pacman_Dies);

                Thread.Sleep(1000);
                RedGhost.Reset();
                BlueGhost.Reset();
                PinkGhost.Reset();
                YellowGhost.Reset();
                Pacman.Reset();
                Bonus = false;
                lives--;
                if (lives == 0)
                {
                    State = GameState.GameOver;
                }
            }
        }

        private void CheckForWin()
        {
            if (dotList.Count == 0 && bonusList.Count == 0)
            {
                parentForm.playSound(Properties.Resources.Pacman_Intermission);
                State = GameState.GamePause;
                Thread.Sleep(5000);
                dotList = PointLists.dotPointList();
                bonusList = PointLists.bonusPointList();
                ChangeLevel();
            }
        }

        private bool IsCollisionWGhost(PacmanCharacter pacman, GhostCharacter ghost)
        {
            List<Point> commonPoints = pacman.GetCore().Intersect(ghost.GetCore().Select(u => u)).ToList();
            if (commonPoints.Count != 0 && ghost.ghostState != GhostState.Eaten)
            {
                return true;
            }
            return false;
        }

        private void OnEatenGhost(PacmanCharacter pacman, GhostCharacter ghost)
        {
            this.score += eatenScore;
            parentForm.playSound(Properties.Resources.Pacman_Eating_Ghost);
            State = GameState.GamePause;
            SetGhostState(ghost.GhostColor, GhostState.Eaten);
            State = GameState.GameRun;
        }

        private void OnEatenPacman(PacmanCharacter pacman, GhostCharacter ghost)
        {
            State = GameState.GamePause;
            parentForm.playSound(Properties.Resources.Pacman_Dies);
            Thread.Sleep(1000);
            RedGhost.Reset();
            BlueGhost.Reset();
            PinkGhost.Reset();
            YellowGhost.Reset();
            Pacman.Reset();
            Bonus = false;
            lives--;
            if (lives == 0)
            {
                State = GameState.GameOver;
            }
        }

        private void CheckCollision(PacmanCharacter pacman)
        {

            for (int i = 0; i < 4; i++)
            {
                GhostCharacter ghost = null;
                switch (i)
                {
                    case 0: ghost = RedGhost; break;
                    case 1: ghost = BlueGhost; break;
                    case 2: ghost = PinkGhost; break;
                    case 3: ghost = YellowGhost; break;
                }
                if (IsCollisionWGhost(pacman, ghost))
                {
                    if (Bonus)
                    {
                        OnEatenGhost(pacman, ghost);
                    }
                    else
                    {
                        OnEatenPacman(pacman, ghost);
                    }
                }
            }
        }


        private GhostColor CheckForCollision()
        {
            eatenScore = (!Bonus || eatenScore > 1600) ? 200 : eatenScore;
            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                if (Ghosts[color].ghostState == GhostState.Eaten)
                {
                    continue;
                }
                List<Point> commonPoints = Pacman.GetCore().Intersect(Ghosts[color].GetCore().Select(u => u)).ToList();
                if (commonPoints.Count != 0 && Bonus)
                {
                    score += eatenScore;
                    eatenScore += eatenScore;
                    parentForm.playSound(Properties.Resources.Pacman_Eating_Ghost);
                    State = GameState.GamePause;
                    SetGhostState(color, GhostState.Eaten);
                    State = GameState.GameRun;
                    return color;
                }
            }
            return GhostColor.None;
        }
        private void ChangeLevel()
        {
            //PacmanSpeed -= 3;
            GhostSpeed -= 3;
            State = GameState.GamePause;
            RedGhost.Reset();
            BlueGhost.Reset();
            PinkGhost.Reset();
            YellowGhost.Reset();
            Pacman.Reset();
            Bonus = false;
            Level++;
        }
        public void SetGhostState(GhostColor color, GhostState state)
        {
            switch (color)
            {
                case GhostColor.Red: RedGhost.ghostState = state; break;
                case GhostColor.Blue: BlueGhost.ghostState = state; break;
                case GhostColor.Pink: PinkGhost.ghostState = state; break;
                case GhostColor.Yellow: YellowGhost.ghostState = state; break;
            }
        }

        public void SetDirection(Direction d)
        {
            Pacman.SetDirection(d);
        }
        public void Stop()
        {
            State = GameState.GameOver;
        }

        public void Pause()
        {
            if (State == GameState.GameRun)
            {
                State = GameState.GamePause;
            }
        }
        public void Continue()
        {
            if (State == GameState.GamePause)
            {
                State = GameState.GameRun;
            }
        }
    }
}
