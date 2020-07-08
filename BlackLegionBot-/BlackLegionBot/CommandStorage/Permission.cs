namespace BlackLegionBot.CommandStorage
{
    public enum EPermission
    {
        ADMIN = 0,
        MODS = 1,
        SUBS = 2,
        EVERYONE = 3
    }

    public static class PermissionExtensions
    {
        /// <summary>
        /// Compares the permission given as an argument (p2) to the permission on which the method was evoked (p1).
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool HasEqualOrHigherPermission(this EPermission p1, EPermission p2) => 
            p2 <= p1;
    }
}