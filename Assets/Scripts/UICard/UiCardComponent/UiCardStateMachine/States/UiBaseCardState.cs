
using Patterns.StateMachine;

namespace Tools.UI.Card
{
    /// <summary>
    ///     Base UI Card State.
    /// </summary>
    public abstract class UiBaseCardState : IState
    {
        private const int SortingOrderOffset = 10;
        private readonly int _originalSortingOrder;

        //--------------------------------------------------------------------------------------------------------------

        #region Constructor

        protected UiBaseCardState(IUiCard handler, BaseStateMachine fsm, UiCardParameters parameters)
        {
            Fsm = fsm;
            Handler = handler;
            Parameters = parameters;

            _originalSortingOrder = Handler.SortingGroup.sortingOrder;

            IsInitialized = true;
        }

        #endregion

        protected IUiCard Handler { get; }
        protected UiCardParameters Parameters { get; }
        protected BaseStateMachine Fsm { get; }
        public bool IsInitialized { get; }

        //--------------------------------------------------------------------------------------------------------------

        #region Operations

        /// <summary>
        ///     Renders the entire card in the first layer.
        /// </summary>
        protected virtual void MakeRenderFirst()
        {
            Handler.SortingGroup.sortingOrder =
                _originalSortingOrder + SortingOrderOffset;
        }

        /// <summary>
        ///     Restores the original sorting order of the card.
        /// </summary>
        protected virtual void MakeRenderNormal()
        {
            Handler.SortingGroup.sortingOrder = _originalSortingOrder;
        }

        /// <summary>
        ///     Enables the card entirely. Collision, Rigidbody and Alpha.
        /// </summary>
        protected void Enable()
        {
            if (Handler.Collider)
                EnableCollision();

            if (Handler.Rigidbody)
                Handler.Rigidbody.Sleep();

            MakeRenderNormal();
            RemoveAllTransparency();
        }

        /// <summary>
        ///     Disables the card entirely. Collision, Rigidbody and Alpha.
        /// </summary>
        protected virtual void Disable()
        {
            DisableCollision();
            Handler.Rigidbody.Sleep();
            MakeRenderNormal();

            foreach (var renderer in Handler.Renderers)
            {
                var myColor = renderer.color;
                myColor.a = Parameters.DisabledAlpha;
                renderer.color = myColor;
            }
        }

        /// <summary>
        ///     Disables the collision with this card.
        /// </summary>
        protected void DisableCollision() => Handler.Collider.enabled = false;

        /// <summary>
        ///     Enables the collision with this card.
        /// </summary>
        protected void EnableCollision() => Handler.Collider.enabled = true;

        /// <summary>
        ///     Remove any alpha channel in all renderers.
        /// </summary>
        protected void RemoveAllTransparency()
        {
            foreach (var renderer in Handler.Renderers)
                if (renderer)
                {
                    var myColor = renderer.color;
                    myColor.a = 1;
                    renderer.color = myColor;
                }
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region FSM

        void IState.OnInitialize()
        {
        }

        public virtual void OnEnterState()
        {
        }

        public virtual void OnExitState()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnNextState(IState next)
        {
        }

        public virtual void OnClear()
        {
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------
    }
}
