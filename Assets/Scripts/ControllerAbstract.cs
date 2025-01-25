using System;
using UnityEngine;
using UnityEngine.Assertions;

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

    public virtual int AddCoins(int count)
    {
        Assert.IsTrue(count > 0);
        Coins += count;
        return Coins;
    }

    public virtual int RemoveCoins(int count)
    {
        Assert.IsTrue(count > 0);
        // TODO: kill the player if not enough?
        Coins -= count;
        return Coins;
    }

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
