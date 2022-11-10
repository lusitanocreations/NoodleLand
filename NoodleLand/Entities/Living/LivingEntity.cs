using System;
using Lusitano.Animations;
using NoodleLand.Enums;
using Unity.VisualScripting;
using UnityEngine;


namespace NoodleLand.Entities.Living
{
    [RequireComponent(typeof(SpriteRenderer),typeof(Animator),typeof(Rigidbody2D))]
    public class LivingEntity : Entity
    {
      [SerializeField] protected Facing facing;
      [SerializeField] private LivingEntityProperties _livingEntityProperties;


      private Rigidbody2D rigidbody;
      private AnimationController _animationController;
      private SpriteRenderer _renderer;
      private Animator _animator;


      public const string AnimationWalkHorizontalTag = "Walk_Horizontal";
      public const string AnimationIdleTag = "Idle";
      public const string AnimationWalkUpTag = "Walk_Up";
      public const string AnimationWalkDownTag = "Walk_Down";

      
      public Facing FacingDirection => facing;


      private void Awake()
      {

          _renderer = GetComponent<SpriteRenderer>();
          _animator = GetComponent<Animator>();
          _animationController = new AnimationController(_animator);
          rigidbody = GetComponent<Rigidbody2D>();

      }

      private void Start()
      {
          rigidbody.gravityScale = 0;
      }

      protected void SetVelocity(Vector2 velocity, float multiplier = 1)
      {
          rigidbody.velocity = velocity * _livingEntityProperties.Speed * multiplier;

      }

      protected void AddToVelocity(Vector2 velocity, float multiplier = 1)
      {
          rigidbody.velocity += velocity * _livingEntityProperties.Speed * multiplier;
      }

      private void ChangeAxis(Facing facing)
      {
        
          switch (facing)
          {
              case Facing.Left:
                  _renderer.flipX = false;
                  return;
              case Facing.Right:
                  _renderer.flipX = true;
                  return;
          }
      }

      protected void PlayAnimation(string animationTag)
      {
          _animationController.PlayAnimation(animationTag);
      }
      protected void SwitchFacing(Facing facing)
      {
          ChangeAxis(facing);
          this.facing = facing;
      }
    }
}
