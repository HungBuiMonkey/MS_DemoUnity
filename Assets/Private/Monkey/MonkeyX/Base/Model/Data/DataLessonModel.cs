public class DataLessonModel
{
    public int IdActivity { get; private set; }
    public int IdGame { get; private set; }
    public DataGameModel DataGameModel { get; private set; }


    public DataLessonModel(int idActivity, int idGame, DataGameModel dataGameModel)
    {
        IdActivity = idActivity;
        IdGame = idGame;
        DataGameModel = dataGameModel;
    }

}
