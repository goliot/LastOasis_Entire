// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("52RqZVXnZG9n52RkZc48h9DUJCY2tOHKAP524zWC6TNxwLMLL/4C6Id3/pecdCs6vOr6BDYRRwJ+A9uuShJevLDk3i61OlvXXjhVoG8rHL+L6cAwwCpd0QPNcMX4MGfpxIq1S1XnZEdVaGNsT+Mt45JoZGRkYGVmWKubNG+jkVxjZH4OG4lMux+XUXl0q2ZLTxlJ6rydjC7CzuNFm8yca5ITfp1ExCqfBJOYmBd5K7buaS2YTPLGSBQGc1G4Wi74K8KToX0Mm5cySaccWmVUCXQAIy5ia/3Lzz52MeVGos7UN6Hujpkky81x5pdkG2qigZ1hrzBLCSbP2JyO21+HOL/mwV3wQ9+6UolftuTimpjJVI1/HGcAHOfosraNiqV+NmdmZGVk");
        private static int[] order = new int[] { 1,7,3,5,12,7,8,9,12,11,10,13,12,13,14 };
        private static int key = 101;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
