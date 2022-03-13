namespace checkers_api.GameModels;

public interface IBoard
{
    public void MakeMove(MoveRequest moveRequest);
    
}