using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeyCode
/// </summary>
public class KeyCode
{
	public int GetKeyCode()
	{
        Random r = new Random();
        int key = r.Next(1000000, 9999999);
        return key;
	}
}