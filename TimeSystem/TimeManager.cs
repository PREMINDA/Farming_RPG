using Script.Enums;
using Script.Events;
using Script.Misc;
using UnityEngine;

namespace Script.TimeSystem
{
    public class TimeManager:SingletoneMb<TimeManager>
    {
        private int _gameYear = 1;
        private Season _gameSeason = Season.Spring;
        private int _gameDay = 1;
        private int _gameHour = 6;
        private int _gameMinute = 30;
        private int _gameSecond = 0;
        private string _gameDayOfWeek = "Mon";

        private bool _gameClockPaused = false;

        private float _gameTick = 0f;
        private void Start()
        {
            EventHandler.CallAdvanceGameMinuteEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
        }
        
        private void Update()
        {
            if (!_gameClockPaused)
            {
                GameTick();
            }
        }
        
        private void GameTick()
        {
            _gameTick += Time.deltaTime;

            if (_gameTick >= Settings.secondsPerGameSecond)
            {
                _gameTick -= Settings.secondsPerGameSecond;
                UpdateGameSecond();
                
            }
        }
        
        private void UpdateGameSecond()
        {
            _gameSecond++;

            if (_gameSecond > 59)
            {
                _gameSecond = 0;
                _gameMinute++;


                if (_gameMinute > 59)
                {
                    _gameMinute = 0;
                    _gameHour++;

                    if (_gameHour > 23)
                    {
                        _gameHour = 0;
                        _gameDay++;

                        if (_gameDay > 30)
                        {
                            _gameDay = 1;

                            int gs = (int)_gameSeason;
                            gs++;

                            _gameSeason = (Season)gs;

                            if (gs > 3)
                            {
                                gs = 0;
                                _gameSeason = (Season)gs;

                                _gameYear++;

                                if (_gameYear > 9999)
                                    _gameYear = 1;


                                EventHandler.CallAdvanceGameYearEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
                            }

                            EventHandler.CallAdvanceGameSeasonEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
                        }

                        _gameDayOfWeek = GetDayOfWeek();
                        EventHandler.CallAdvanceGameDayEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
                    }

                    EventHandler.CallAdvanceGameHourEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
                }

                EventHandler.CallAdvanceGameMinuteEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);

            }

            // Call to advance game second event would go here if required
        }
        private string GetDayOfWeek()
        {
            int totalDays = (((int)_gameSeason) * 30) + _gameDay;
            int dayOfWeek = totalDays % 7;

            switch (dayOfWeek)
            {
                case 1:
                    return "Mon";

                case 2:
                    return "Tue";

                case 3:
                    return "Wed";

                case 4:
                    return "Thu";

                case 5:
                    return "Fri";

                case 6:
                    return "Sat";

                case 0:
                    return "Sun";

                default:
                    return "";
            }
        }
    }
    
    
}