using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PacmanWindowForms.Scripts.Models;
using Microsoft.VisualBasic.Logging;


namespace PacmanWindowForms.Scripts.Models_refactor
{
    public class GameBoardView
    {
        private string TAG = "GameBoardView";
        public GameBoardView() { }

        public void onInit()
        {
            Logger.Log(TAG + "onInit");
        }

        public void onEntityMove(EntityType type)
        {
            switch (type)
            {
                case EntityType.Pacman:
                    {
                        Logger.Log(TAG + "onEntityMove: Pacman - Update pacman view");
                        break;
                    }
                case EntityType.Ghost:
                    {
                        Logger.Log(TAG + "onEntityMove: Ghost - Update ghost view");
                        break;
                    }
                default:
                    {
                        Logger.Log(TAG + "onEntityMove: Unknown entity type" + type.ToString());
                        break;
                    }
            }
        }
    }
}