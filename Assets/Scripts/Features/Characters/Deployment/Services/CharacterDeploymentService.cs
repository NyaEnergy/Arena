using UnityEngine;

public class CharacterDeploymentService {
    private readonly CharacterFactory _characterFactory;
    private readonly SummonerFactory _summonerFactory;

    private readonly CharacterDeploymentPositionService _positionService;

    public CharacterDeploymentService(CharacterFactory characterFactory,
                                      SummonerFactory summonerFactory,
                                      CharacterDeploymentPositionService positionService) {
        _characterFactory = characterFactory;
        _summonerFactory = summonerFactory;
        _positionService = positionService;
    }

    public CharacterView Deploy(CharacterDeploymentRequest request) {
        if (request == null ||
            !request.IsValid) {
            return null;
        }

        if (!_positionService.TryGetPosition(request.TeamType,
                                         out Vector3 position)) {
            return null;
        }

        return Deploy(request, position);
    }

    public CharacterView Deploy(CharacterDeploymentRequest request,
                                Vector3 position) {
        if (request == null ||
           !request.IsValid) return null;

        CharacterView view = Create(request, position);

        if (view == null) return null;

        view.transform.rotation = _positionService.GetRotation(request.TeamType);

        return view;
    }

    private CharacterView Create(CharacterDeploymentRequest request,
                                 Vector3 position) {
        if (request.IsSummoner) {
            return _summonerFactory.Spawn(
                        request.TeamType,
                        request.SummonerConfig,
                        position);
        }

        CharacterKey key = new(request.TeamType,
                               request.CharacterType);

        return _characterFactory.Spawn(key, position);
    }
}