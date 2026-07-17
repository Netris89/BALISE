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

        this.WhenActivated(disposable =>
        {
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.FrenchButton).DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.DutchButton).DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.GermanButton).DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.EnglishButton).DisposeWith(disposable);
        });
    }
}