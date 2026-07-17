using System.Reactive.Disposables.Fluent;
using Balise.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Balise.Views;

public partial class LangChoiceView : ReactiveUserControl<LangChoiceViewModel>
{
    public LangChoiceView()
    {
        InitializeComponent();
    }
}