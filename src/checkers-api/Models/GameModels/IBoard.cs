namespace checkers_api.Models.GameModels;

public interface IBoard
{
    public void MakeMove(MoveRequest moveRequest);
    
}