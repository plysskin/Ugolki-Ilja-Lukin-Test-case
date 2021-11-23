/* WILL ADD LATER
 *
 * TODO:ADD TEST
 *
 *
 
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Ugolki;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions


        var l1 = new Location() { x = 1, y = 2 };
        var l2 = new Location() { x = 1, y = 2 };
        var l3 = new Location() { x = 4, y = 3 };
        Location l4 = new Location(6, 6);
        GameObject o1 = null;
        GameObject o2 = null;



        Debug.Log("Hi!");
        Debug.Log($"l1==l2:{l1 == l2} l1==l3:{l1 == l3} null==null {null == null} l3==l3: {l3 == l3}");

        Debug.Log($"l4==l1:{l4 == l1} l4==l4: {l4 == l4} l4==null: {l4 == null}");
        Debug.Log($"o1==o2 {o1 == o2} l1.Equals(l2):{l1.Equals(l2)} l1.Equals(l3):{l1.Equals(l3)}");
        Debug.Log("===================================================================================");
        var m1 = new Move(l1, l3);
        var m2 = new Move(l1, l3);
        var m3 = new Move(l3, l1);
        //        var m4 = new Move(new Location(1, 2), new Location(4, 3));

        Debug.Log($"m1==m2:{m1 == m2} m1==m3:{m1 == m3} null==null {null == null} m3==m3: {m3 == m3}");
        //      Debug.Log($"m4==m1:{m4 == m1} m4==m4: {m4 == m4} m4==null: {m4 == null} ");

        Assert.Equals(l1, l2);
        Assert.True(l1==l2);
        Assert.False(l1==l3);
        Assert.True(m1==m2);
        Assert.False(m1==m4);


    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
*/