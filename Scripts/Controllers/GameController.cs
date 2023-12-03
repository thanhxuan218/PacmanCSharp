using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Timers;
using PacmanWindowForms.Script.Models;
using PacmanWindowForms.Forms;
using PacmanWindowForms.Script.Views;
using static System.Windows.Forms.AxHost;
using System.Diagnostics;

namespace PacmanWindowForms.Script.Controllers
{
    public class GameController
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
        public int GhostSpeed
        {
            get { return _ghostSpeed; }
            set
            {
                _ghostSpeed = (value < 10) ? 10 : value;
                Ghosts[GhostColor.Red].speed = _ghostSpeed;
                Ghosts[GhostColor.Blue].speed = _ghostSpeed;
                Ghosts[GhostColor.Pink].speed = _ghostSpeed;
                Ghosts[GhostColor.Yellow].speed = _ghostSpeed;
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
                Ghosts[GhostColor.Red].gameState = value;
                Ghosts[GhostColor.Blue].gameState = value;
                Ghosts[GhostColor.Pink].gameState = value;
                Ghosts[GhostColor.Yellow].gameState = value;
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

        private readonly frmGameBoard parentForm;
        private readonly PacmanBoard board;
        private readonly PacmanCharacter Pacman;
        private StaticEntity staticEntity;
        private Dictionary<GhostColor, Ghost> Ghosts = new Dictionary<GhostColor, Ghost>() {
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


        public GameController(frmGameBoard frm, Panel p)
        {
            parentForm = frm;
            board = new PacmanBoard(p);
            Pacman = new PacmanCharacter(board);
            Ghosts[GhostColor.Red] = new Ghost(board, GhostColor.Red);
            Ghosts[GhostColor.Blue] = new Ghost(board, GhostColor.Blue);
            Ghosts[GhostColor.Pink] = new Ghost(board, GhostColor.Pink);
            Ghosts[GhostColor.Yellow] = new Ghost(board, GhostColor.Yellow);
            staticEntity = new StaticEntity(board);
            this.State = GameState.GamePaused;
            this.Initialize();
            RePaint();
        }


        private void Initialize()
        {
            MapLoader.Instance.LoadBoard();

            wallList = MapLoader.Instance.WallList();
            dotList = MapLoader.Instance.DotList();
            boxList = MapLoader.Instance.BoxList();
            boxDoorList = MapLoader.Instance.BoxDoorList();
            bonusList = MapLoader.Instance.BonusList();

            staticEntity.Initialize();
            Pacman.Initialize();
            Ghosts[GhostColor.Red].Initialize();
            Ghosts[GhostColor.Blue].Initialize();
            Ghosts[GhostColor.Pink].Initialize();
            Ghosts[GhostColor.Yellow].Initialize();

            //MessageBox.Show($"wallList {wallList.Count}, dotList {dotList.Count}, boxList {boxList.Count}, boxDoorList {boxDoorList.Count}, bonusList {bonusList.Count}");
            State = GameState.GameOver;
            score = 0;
            PacmanSpeed = 80;
            GhostSpeed = 80;
        }


        public void Run()
        {
            RePaint();
            Runner = new Task(runGame);
            Runner.Start();
            staticEntity.StaticEntityStart();
            MonitorBonusBehavior();
            Pacman.Run();
            Ghosts[GhostColor.Red].Run();
            Ghosts[GhostColor.Blue].Run();
            Ghosts[GhostColor.Pink].Run();
            Ghosts[GhostColor.Yellow].Run();
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
                    EatDots(Pacman.GetCore());
                    EatBonus(Pacman.GetCore());
                    AddLives();
                    staticEntity.BonusClear();
                    CheckForWin();
                    CheckCollision(this.Pacman);
                    parentForm.Show(score.ToString(), Level.ToString(), lives);
                    Runner.Wait(5);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private void AddLives()
        {
            if (score % 10000 == 0)
            {
                //parentForm.playSound(Properties.Resources.Pacman_Extra_Live);
                score += 10;
                lives++;
            }
        }
        // Use timer instead of this
        private void MonitorBonusBehavior()
        {
            this.TimerInitialize();
            bonusMonitorTask = new Task(() =>
            {
                while (State != GameState.GameOver)
                {
                    try
                    {
                        ChangeGhostState();
                        BonusStateChange();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    bonusMonitorTask.Wait(100);
                }
            });
            bonusMonitorTask.Start();
        }

        private Dictionary<GhostColor, System.Timers.Timer> ghostStateTimers = new Dictionary<GhostColor, System.Timers.Timer>()
        {
            { GhostColor.Blue, null },
            { GhostColor.Pink, null },
            { GhostColor.Red, null },
            { GhostColor.Yellow, null }
        };

        private void TimerInitialize()
        {
            // Initialize the ghost state timers
            ghostStateTimers[GhostColor.Blue] = new System.Timers.Timer(50);
            ghostStateTimers[GhostColor.Blue].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Blue].AutoReset = true;

            ghostStateTimers[GhostColor.Pink] = new System.Timers.Timer(50);
            ghostStateTimers[GhostColor.Pink].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Pink].AutoReset = true;

            ghostStateTimers[GhostColor.Red] = new System.Timers.Timer(50);
            ghostStateTimers[GhostColor.Red].Elapsed += GhostStateTimerElapsed;
            ghostStateTimers[GhostColor.Red].AutoReset = true;

            ghostStateTimers[GhostColor.Yellow] = new System.Timers.Timer(50);
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
                    if (Ghosts[color].GhostState == GhostState.Bonus)
                    {
                        // Change the ghost state to normal
                        SetGhostState(color, GhostState.Normal);
                    }
                    else if (Ghosts[color].GhostState == GhostState.BonusEnd)
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
                if (Ghosts[color].GhostState == GhostState.Eaten && this.State == GameState.GameOn)
                {
                    ghostStateTimers[color].Start();
                }
                else
                {
                    ghostStateTimers[color].Stop();
                }
            }
        }

        int bonusStateCounter = 0;
        int bonusCounter = 0;
        bool BonusEndSprite = true;
        private void BonusStateChange()
        {
            bonusStateCounter = (bonusStateCounter > 4) ? 1 : bonusStateCounter;
            bonusStateCounter++;
            if (Bonus && bonusCounter < 70 && this.State == GameState.GameOn)
            {
                bonusCounter++;

            }
            else if (Bonus && bonusCounter >= 70 && bonusCounter < 100)
            {
                if (BonusEndSprite)
                {
                    foreach (GhostColor color in ghostStateTimers.Keys)
                    {
                        if (Ghosts[color].GhostState != GhostState.Eaten)
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
                        if (Ghosts[color].GhostState != GhostState.Eaten)
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
                    if (Ghosts[color].GhostState != GhostState.Eaten)
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
            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                mergedList = mergedList.Union(Ghosts[color].GetCore()).ToList();
            }

            List<Point> commonPoints = Pacman.GetCore().Intersect(mergedList.Select(u => u)).ToList();

            if ((commonPoints.Count != 0 && !Bonus) || lives == 0)
            {
                State = GameState.GamePaused;
                parentForm.PlaySound(new MemoryStream(Properties.Resources.Pacman_Dies));

                Thread.Sleep(1000);
                foreach (GhostColor color in ghostStateTimers.Keys)
                {
                    this.Ghosts[color].Reset();
                }
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
                parentForm.PlaySound(Properties.Resources.Pacman_Intermission);
                State = GameState.GamePaused;
                Thread.Sleep(5000);
                dotList = MapLoader.Instance.DotList();
                bonusList = MapLoader.Instance.BonusList();
                ChangeLevel();
            }
        }

        private bool IsCollisionWGhost(PacmanCharacter pacman, Ghost ghost)
        {
            List<Point> commonPoints = pacman.GetCore().Intersect(ghost.GetCore().Select(u => u)).ToList();
            if (commonPoints.Count != 0 && ghost.GhostState != GhostState.Eaten)
            {
                return true;
            }
            return false;
        }

        private void OnEatenGhost(PacmanCharacter pacman, Ghost ghost)
        {
            this.score += eatenScore;
            parentForm.PlaySound(Properties.Resources.Pacman_Eating_Ghost);
            State = GameState.GamePaused;
            SetGhostState(ghost.Color, GhostState.Eaten);
            State = GameState.GameOn;
        }

        private void OnEatenPacman(PacmanCharacter pacman, Ghost ghost)
        {
            State = GameState.GamePaused;
            parentForm.PlaySound(new MemoryStream(Properties.Resources.Pacman_Dies));
            Thread.Sleep(1000);
            GhostReset();
            Pacman.Reset();
            Bonus = false;
            lives--;
            if (lives == 0)
            {
                State = GameState.GameOver;
            }
        }

        private void GhostReset()
        {
            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                this.Ghosts[color].Reset();
            }
        }

        private void CheckCollision(PacmanCharacter pacman)
        {

            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                if (IsCollisionWGhost(pacman, this.Ghosts[color]))
                {
                    if (Bonus)
                    {
                        OnEatenGhost(pacman, this.Ghosts[color]);
                    }
                    else
                    {
                        OnEatenPacman(pacman, this.Ghosts[color]);
                    }
                }
            }
        }


        private GhostColor CheckForCollision()
        {
            eatenScore = (!Bonus || eatenScore > 1600) ? 200 : eatenScore;
            foreach (GhostColor color in ghostStateTimers.Keys)
            {
                if (Ghosts[color].GhostState == GhostState.Eaten)
                {
                    continue;
                }
                List<Point> commonPoints = Pacman.GetCore().Intersect(Ghosts[color].GetCore().Select(u => u)).ToList();
                if (commonPoints.Count != 0 && Bonus)
                {
                    score += eatenScore;
                    eatenScore += eatenScore;
                    parentForm.PlaySound(Properties.Resources.Pacman_Eating_Ghost);
                    State = GameState.GamePaused;
                    SetGhostState(color, GhostState.Eaten);
                    State = GameState.GameOn;
                    return color;
                }
            }
            return GhostColor.None;
        }
        private void ChangeLevel()
        {
            //PacmanSpeed -= 3;
            GhostSpeed -= 3;
            State = GameState.GamePaused;
            GhostReset();
            Pacman.Reset();
            Bonus = false;
            Level++;
        }
        public void SetGhostState(GhostColor color, GhostState state)
        {
            Ghosts[color].GhostState = state;
        }

        public void SetDirection(Direction d)
        {
            if (State == GameState.GameOn)
            {
                Debug.WriteLine($"Set Direction {d} for Pacman");
                Pacman.SetDirection(d);
            }
        }
        public void Stop()
        {
            State = GameState.GameOver;
        }

        public void Pause()
        {
            if (State == GameState.GameOn)
            {
                State = GameState.GamePaused;
            }
        }
        public void Resume()
        {
            if (State == GameState.GamePaused)
            {
                State = GameState.GameOn;
            }
        }
    }
}
