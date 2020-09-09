using UnityEngine;

namespace PixelAdventure.API
{
    public class AnimatorHashes
    {
        public static readonly int COLLECTED     = Animator.StringToHash("collected");
        public static readonly int BLINK         = Animator.StringToHash("blink");
        public static readonly int PUSH          = Animator.StringToHash("push");
        public static readonly int IDLE          = Animator.StringToHash("idle");
        public static readonly int HIT           = Animator.StringToHash("hit");
        public static readonly int OFF           = Animator.StringToHash("off");
        public static readonly int ON            = Animator.StringToHash("on");

        public static readonly int DOUBLE_JUMP   = Animator.StringToHash("isDoubleJumping");
        public static readonly int WALL_SLIDING  = Animator.StringToHash("wallSliding");
        public static readonly int Y_VELOCITY    = Animator.StringToHash("yVelocity");
        public static readonly int GROUNDED      = Animator.StringToHash("isGrounded");
        public static readonly int RUN           = Animator.StringToHash("isRunning");

        public static readonly int BOTTOM_HIT    = Animator.StringToHash("bottomHit");
        public static readonly int RIGHT_HIT     = Animator.StringToHash("rightHit");
        public static readonly int LEFT_HIT      = Animator.StringToHash("leftHit");
        public static readonly int TOP_HIT       = Animator.StringToHash("topHit");
    }
}
