namespace AshGreen.Player
{
    public enum PlayerStateType
    {
        Null = -1,
        //기본 상태
        OnGround = 0, OnAir = 1, Hit = 2, Death = 3, Move = 4, Jump = 5, DownJump = 6,
        //주력 스킬
        MainSkill = 101,
        //보조 스킬
        SecondarySkill = 201,
        //특수 스킬
        SpecialSkill = 301
    }
}