using LootLocker.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;

namespace Assets.Scripts
{
    public static class LeaderboardManager
    {
        public static bool isLoggedIn = false;
        private static string leaderboardKey = "allTimeHighScore";

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit. this enforces that the class is initialized only when the first static member is accessed
        static LeaderboardManager()
        {
        }

        private static async Task AsyncLootLockerLogin()
        {
            var done = false;
            isLoggedIn = false;
            LootLockerSDKManager.StartGuestSession((response) =>
                {
                    isLoggedIn = HandleLoginResponse(response);
                    done = true;
                });

            while (!done)
            {
                await Task.Yield();
            }
            Debug.Log($"LeaderBoardManager - Guest player logged state. Id={PlayerPrefs.GetString("PlayerID")}, isLoggedIn={isLoggedIn}, Done:{done}");
        }

        private static bool HandleLoginResponse(LootLockerGuestSessionResponse response)
        {
            if (!response.success)
            {
                Debug.Log($"LeaderBoardManager - Failed to login as guest. Error:{response.Error}");
                return false;
            }

            PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
            return true;
        }

        public static async Task AsyncSubmitScore(int score)
        {
            if (!isLoggedIn)
            {
                await AsyncLootLockerLogin();
            }

            var done = false;
            var playerId = PlayerPrefs.GetString("PlayerID");

            LootLockerSDKManager.SubmitScore(playerId, score, leaderboardKey, (response) =>
            {
                if (!response.success)
                {
                    Debug.Log($"LeaderBoardManager - Failed to submit score. Error:{response.Error}");
                }
                done = true;
            });

            while (!done)
            {
                await Task.Yield();
            }
            Debug.Log($"LeaderBoardManager - Submit Score = {score}, isLoggedIn={isLoggedIn}, Done:{done}");
        }

        public static async Task<LootLockerLeaderboardMember[]> AsyncGetLeaderboard()
        {
            if (!isLoggedIn)
            {
                await AsyncLootLockerLogin();
            }

            var done = false;
            LootLockerLeaderboardMember[] board = null;
            LootLockerSDKManager.GetScoreList(leaderboardKey, 100, 0, (response) =>
            {
                if (response.success)
                {
                    board = response.items;
                }
                else
                {
                    Debug.Log($"LeaderBoardManager - Failed to fetch leader board. Error:{response.Error}");
                }
                done = true;
            });
            
            while (!done)
            {
                await Task.Yield();
            }
            
            Debug.Log($"LeaderBoardManager - Fetch board, isLoggedIn={isLoggedIn}, Done:{done}");
            return board;
        }

    }
}
