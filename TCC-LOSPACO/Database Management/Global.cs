public static class Global {
    static public object FormatArray(object[] comps, int style) {
        string txt = "", comma = "", qm = "";
        if (style >= 0) comma = ", ";
        if (style >= 1) qm = "'";
        for (int i = 0; i < comps.Length; i++) {
            object c = comps[i];
            txt += i != comps.Length - 1 ? $"{qm}{c}{qm}{comma}" : $"{qm}{c}{qm}";
        }
        return txt;
    }

    static public object GetValue(object obj, string value) { return obj.GetType().GetProperty(value)?.GetValue(obj, null); }
}