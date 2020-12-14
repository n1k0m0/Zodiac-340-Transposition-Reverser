# Zodiac-340-Transposition-Reverser
This repository contains a small program, that allows to reverse the used transposition cipher, which the Zodiac Killer used in his Z-340 message.

To reverse the substitution (after the transposition), you can use AZDecrypt (from van Eycke) or you can use our CrypTool 2
* AZDecrypt: http://zodiackillersite.com/viewtopic.php?f=81&t=3198
* CrypTool 2: https://www.cryptool.org/de/ct2/downloads

If you want to use CrypTool 2 to decipher the remaining substitution cipher, after reversing the transposition, you can find in CT2_workspace a file named "z340_decrypt_substitution.cwm", which you can load and execute in CrypTool 2. The workspace already contained the intermediate ciphertext and the correct substitution key.

For details, have a look at David Oranchak's video about breaking the cipher: https://www.youtube.com/watch?v=-1oQLPRE21o
Also, I made a video about breaking the first block of the cipher to verify their solution: https://www.youtube.com/watch?v=hIrOftXgibg

Kind regards,
Nils
