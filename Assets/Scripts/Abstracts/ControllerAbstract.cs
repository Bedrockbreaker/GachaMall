using System;
using UnityEngine;

public abstract class ControllerAbstract : MonoBehaviour
{

    private bool isUpdatingInternal = false;

    [field: SerializeField]
    public PawnAbstract Pawn { get; protected set; }
    [field: SerializeField]
    public int Coins { get; protected set; } = 0;

    public event Action<PawnAbstract> OnPossess;
    public event Action OnUnPossess;

    protected abstract void HandleInput();

    public virtual bool Possess(PawnAbstract pawn)
    {
        if (isUpdatingInternal) return false;
        isUpdatingInternal = true;

        if (Pawn != null) {
            OnUnPossess?.Invoke();
            Pawn.UnbindController();
        }

        Pawn = pawn;

        if (pawn != null) {
            OnPossess?.Invoke(pawn);
            pawn.BindController(this);
        }

        isUpdatingInternal = false;
        return true;
    }

    public virtual bool Unpossess() => Possess(null);

    public virtual void Start()
    {
        GameManager.Instance.RegisterController(this);
    }

    public virtual void Update()
    {
        HandleInput();
    }

    public virtual void OnDestroy()
    {
        Unpossess();
        GameManager.Instance.UnregisterController(this);
    }
}
