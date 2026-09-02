using System;
using ReactiveUI;

namespace Balise.Services;

public class AgeService : ReactiveObject
{
    private bool isChildren;
    private bool isDisabled;

    public bool IsChildren
    {
        get => isChildren;
        set => this.RaiseAndSetIfChanged(ref isChildren, value);
    }

    public bool IsDisabled
    {
        get => isDisabled;
        set => this.RaiseAndSetIfChanged(ref isDisabled, value);
    }
}
