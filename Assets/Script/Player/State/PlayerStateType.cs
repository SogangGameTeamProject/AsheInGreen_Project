namespace AshGreen.Player
{
    public enum PlayerStateType
    {
        Null = -1,
        //기본 상태
        Idle = 0, Hit = 1, Death = 2, Move = 3, Jump = 4,
        //주력 스킬
        MainSkill = 101,
        //보조 스킬
        SecondarySkill = 201,
        //특수 스킬
        SpecialSkill = 301
    }
}