using System;

namespace PortalCiudadano.Helpers
{
    public class EncriptId
    {
        public static string EncodeId(int id)
        {
            byte[] idBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString());
            return Convert.ToBase64String(idBytes);
        }

        public static int DecodeId(string encodedId)
        {
            byte[] idBytes = Convert.FromBase64String(encodedId);
            string idString = System.Text.Encoding.UTF8.GetString(idBytes);
            return int.Parse(idString);
        }

    }
}