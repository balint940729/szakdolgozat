public static class ExtraTurnHandler {
    private static bool extraTurn;

    public static bool IsExtraTurn() {
        return extraTurn;
    }

    public static void SetExtraTurn() {
        extraTurn = true;
    }

    public static void ResetExtraTurn() {
        extraTurn = false;
    }
}