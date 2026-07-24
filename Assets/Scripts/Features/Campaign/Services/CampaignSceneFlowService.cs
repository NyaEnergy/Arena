using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class CampaignSceneFlowService {
    private readonly CampaignRuntime _runtime;
    private readonly CampaignService _campaignService;

    private string _campaignSceneName;

    public CampaignSceneFlowService(CampaignRuntime runtime,
                                    CampaignService campaignService) {
        _runtime = runtime ??
            throw new ArgumentNullException(nameof(runtime));
        _campaignService = campaignService ??
            throw new ArgumentNullException(nameof(campaignService));
    }

    public bool CanEnterCurrentTerritory() {
        StoryTerritoryConfig territory =
            _runtime.CurrentTerritory;
        Scene activeScene =
            SceneManager.GetActiveScene();

        return _runtime.State == CampaignState.TerritoryReady &&
               territory != null &&
               territory.IsValid &&
               activeScene.IsValid() &&
               !string.IsNullOrWhiteSpace(activeScene.name) &&
               !string.Equals(
                   activeScene.name,
                   territory.SceneName,
                   StringComparison.Ordinal) &&
               Application.CanStreamedLevelBeLoaded(
                   territory.SceneName);
    }

    public bool TryEnterCurrentTerritory() {
        if (!CanEnterCurrentTerritory()) return false;

        StoryTerritoryConfig territory =
            _runtime.CurrentTerritory;
        Scene campaignScene =
            SceneManager.GetActiveScene();

        if (!_campaignService.TryBeginTerritory()) {
            return false;
        }

        _campaignSceneName = campaignScene.name;

        SceneManager.LoadScene(
            territory.SceneName,
            LoadSceneMode.Single);

        return true;
    }

    public bool TryReturnToCampaign() {
        if (!CanReturnToCampaign()) return false;

        SceneManager.LoadScene(
            _campaignSceneName,
            LoadSceneMode.Single);

        return true;
    }

    private bool CanReturnToCampaign() {
        bool isTerritoryFinished =
            _runtime.State == CampaignState.TerritoryReady ||
            _runtime.State == CampaignState.ArcCompleted;

        if (!isTerritoryFinished ||
            string.IsNullOrWhiteSpace(
                _campaignSceneName)) return false;

        Scene activeScene =
            SceneManager.GetActiveScene();

        return activeScene.IsValid() &&
               !string.Equals(
                   activeScene.name,
                   _campaignSceneName,
                   StringComparison.Ordinal) &&
               Application.CanStreamedLevelBeLoaded(
                   _campaignSceneName);
    }
}
