public interface ICameraState
{
    void Enter(CameraController controller);
    void Exit(CameraController controller);
    void Update(CameraController controller);
    void HandleInput(CameraController controller);
}