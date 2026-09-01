using ConveyorWars.Presentation.Units;
using UnityEngine;

namespace ConveyorWars.Presentation.Input {
    public readonly struct GameplayCommand {
        public GameplayCommandType Type { get; }
        public Vector3 WorldPoint { get; }
        public UnitView UnitView { get; }

        private GameplayCommand(
            GameplayCommandType type,
            Vector3 worldPoint,
            UnitView unitView) {
            Type = type;
            WorldPoint = worldPoint;
            UnitView = unitView;
        }

        public static GameplayCommand CreateMove(
            Vector3 worldPoint) {
            return new GameplayCommand(
                GameplayCommandType.Move,
                worldPoint,
                null);
        }

        public static GameplayCommand CreateUnitInteraction(
            UnitView unitView) {
            return new GameplayCommand(
                GameplayCommandType.UnitInteraction,
                default,
                unitView);
        }
    }
}