using System;

namespace CapybaraToolkit.Xliff12
{
    public enum State
    {
        None,
        New,
        Translated,
        SignedOff,
        NeedsAdaptation,
        NeedsL10n,
        NeedsReviewAdaptation,
        NeedsReviewL10n,
        NeedsReviewTranslation,
        NeedsTranslation,
        Final
    }

    public static class StateExtensions
    {
        public static string StateToString(this State state) => state switch
        {
            State.None => "",
            State.New => "new",
            State.Translated => "translated",
            State.SignedOff => "signed-off",
            State.NeedsAdaptation => "needs-adaptation",
            State.NeedsL10n => "needs-l10n",
            State.NeedsReviewAdaptation => "needs-review-adaptation",
            State.NeedsReviewL10n => "needs-review-l10n",
            State.NeedsReviewTranslation => "needs-review-translation",
            State.NeedsTranslation => "needs-translation",
            State.Final => "final",
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };

        public static State StringToState(this string value) => value switch
        {
            "" => State.None,
            null => State.None,
            "new" => State.New,
            "translated" => State.Translated,
            "signed-off" => State.SignedOff,
            "needs-adaptation" => State.NeedsAdaptation,
            "needs-l10n" => State.NeedsL10n,
            "needs-review-adaptation" => State.NeedsReviewAdaptation,
            "needs-review-l10n" => State.NeedsReviewL10n,
            "needs-review-translation" => State.NeedsReviewTranslation,
            "needs-translation" => State.NeedsTranslation,
            "final" => State.Final,
            _ => throw new ArgumentOutOfRangeException(value)
        };
    }
}
