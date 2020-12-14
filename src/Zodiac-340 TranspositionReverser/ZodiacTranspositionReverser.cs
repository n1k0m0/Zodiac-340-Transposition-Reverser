/*                              
   Copyright 2020 Nils Kopal, nils.kopal<AT>cryptool.org

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Text;

namespace ZodiacTranspositionReverser
{
    /// <summary>
    /// This program reverses the transposition cipher, which the Zodiac killer used as first step with the Z-340 cipher
    /// The ciphertext was originally broken by David Oranchak, Jarl van Eycke, and Sam Black in December 2020
    /// 
    /// To reverse the substitution (after the transposition), you can use AZDecrypt (from van Eycke) or you can use our CrypTool 2
    /// AZDecrypt: http://zodiackillersite.com/viewtopic.php?f=81&t=3198
    /// CrypTool 2: https://www.cryptool.org/de/ct2/downloads
    /// 
    /// For details, have a look at David Oranchak's video about breaking the cipher: https://www.youtube.com/watch?v=-1oQLPRE21o
    /// Also, I made a video about breaking the first block of the cipher to verify their solution: https://www.youtube.com/watch?v=hIrOftXgibg
    /// </summary>
    public class ZodiacTranspositionReverser
    {        
        public static void Main(string[] args)
        {
            //transcription of Z-340: Obtained from: http://zodiackillerciphers.com/wiki/index.php?title=Cipher_comparisons
            string Z_340 =  "HER>pl^VPk|1LTG2d" + // 
                            "Np+B(#O%DWY.<*Kf)" + //
                            "By:cM+UZGW()L#zHJ" + //
                            "Spp7^l8*V3pO++RK2" + //
                            "_9M+ztjd|5FP+&4k/" + //
                            "p8R^FlO-*dCkF>2D(" + //
                            "#5+Kq%;2UcXGV.zL|" + //
                            "(G2Jfj#O+_NYz+@L9" + //
                            "d<M+b+ZR2FBcyA64K" + // <- until here: first block (Z-340-1)
                            "-zlUV+^J+Op7<FBy-" + //
                            "U+R/5tE|DYBpbTMKO" + //
                            "2<clRJ|*5T4M.+&BF" + //
                            "z69Sy#+N|5FBc(;8R" + //
                            "lGFN^f524b.cV4t++" + //
                            "yBX1*:49CE>VUZ5-+" + //
                            "|c.3zBK(Op^.fMqG2" + //
                            "RcT+L16C<+FlWB|)L" + //
                            "++)WCzWcPOSHT/()p" + // <- until here: second block (Z-340-2)
                            "|FkdW<7tB_YOB*-Cc" + //
                            ">MDHNpkSzZO8A|K;+";  // <- until here: third block (Z-340-3)
                        
            var builder = new StringBuilder();

            //Step 1: Read out first block of intermediate ciphertext
            builder.Append(ReadOutTranspositionPartOne(Z_340));
            //Step 2: Read out second block of intermediate ciphertext
            builder.Append(ReadOutTranspositionPartTwo(Z_340));
            //Step 3: Read out last block of intermediate ciphertext
            builder.Append(ReadOutTranspositionPartThree(Z_340));

            //I replace the ; by Ä and I replace the | by Ö, since the CrypTool 2 homophonic substitution analyzer
            //and the substitution component use these symbols in their key syntax
            Console.WriteLine(builder.ToString().Replace(";","Ä").Replace("|", "Ö"));
            Console.ReadLine();
        }

        /// <summary>
        /// Reads out the first block of the intermediate ciphertext
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static string ReadOutTranspositionPartOne(string ciphertext)
        {
            //get first block to read out
            ciphertext = ciphertext.Substring(0, 153);

            int x = 0;
            int y = 0;
            int c = 0;

            var builder = new StringBuilder();

            while (true)
            {                
                builder.Append(ciphertext[x + y * 17]);

                x = (x + 2) % 17;
                y = (y + 1) % 9;
                c++;

                if (c == ciphertext.Length)
                {
                    break;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Reads out the second block of the intermediate ciphertext and
        /// - ignores the non-transposed "LIFEIS"-part
        /// - fixes the shift error which the Zodiac made during the encryption process
        /// - reads out non-transposed "LIFEIS"-part after reverting transposition
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static string ReadOutTranspositionPartTwo(string ciphertext)
        {
            //get first block to read out
            ciphertext = ciphertext.Substring(153, 153);

            //move symbol from end of 6th line to 3rd position of 6th line => fixes shift error
            ciphertext = ciphertext.Insert(88, string.Format("{0}", ciphertext[101]));
            ciphertext = ciphertext.Remove(102, 1);

            int x = 0;
            int y = 0;
            int c = 0;

            var builder = new StringBuilder();

            while (true)
            {
                //ignore non-transposed "LIFEIS"-part of this ciphertext
                //starting at x-offset 11 and y-offset 0 and having length 6
                if(!(y == 0 && x >=11 && x < 17))
                {
                    builder.Append(ciphertext[x + y * 17]);                    
                }
                x = (x + 2) % 17;
                y = (y + 1) % 9;
                c++;

                if (c == ciphertext.Length)
                {
                    break;
                }

            }

            //read out non transposed part
            for(int i = 11; i < 17; i++)
            {
                builder.Append(ciphertext[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Reads out the third and last block of the intermediate ciphertext
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static string ReadOutTranspositionPartThree(string ciphertext)
        {
            //reads out last block as is
            //keep in mind, that some words after substitution are somehow still "reversed"
            //I assume, this may be also intentional and has to be read out later using following a rule: 
            //-->first a word in correct order, then reversed, then correct, then reversed, ... and so on
            //OR: maybe, Zodiac also made an error here?
            return ciphertext.Substring(153 + 153);
        }
    }
}
