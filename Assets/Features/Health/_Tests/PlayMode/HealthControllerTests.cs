using System.Collections;
using Features.Health;
using Features.Health.Events;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class HealthControllerTests
{
    [UnityTest]
    public IEnumerator ItIncreasesHealthWhenHealing()
    {
        var gameObject = new GameObject();

        var comp = gameObject.AddComponent<HealthController>();

        int healthBefore = comp.CurrentHealth;

        int healAmount = 1;

        comp.Heal(healAmount);

        Assert.AreEqual(healthBefore + healAmount, comp.CurrentHealth);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItLowersHealthWhenDamaged()
    {
        var gameObject = new GameObject();

        var comp = gameObject.AddComponent<HealthController>();

        int healthBefore = comp.CurrentHealth;

        int damageAmount = 1;

        comp.Damage(damageAmount);

        Assert.AreEqual(healthBefore - damageAmount, comp.CurrentHealth);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItCallsDamageInterceptorCallbacks()
    {
        var gameObject = new GameObject();

        var comp = gameObject.AddComponent<HealthController>();

        int counter = 0;

        comp.OnBeforeDamage += x =>
        {
            counter++;
            return new HealthChangeInterceptedEventArgs(x, 0);
        };

        comp.Damage(1);

        Assert.AreEqual(1, counter);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItCallsHealInterceptorCallbacks()
    {
        var gameObject = new GameObject();

        var comp = gameObject.AddComponent<HealthController>();

        int counter = 0;

        comp.OnBeforeHeal += x =>
        {
            counter++;
            return new HealthChangeInterceptedEventArgs(x, 0);
        };

        comp.Heal(1);

        Assert.AreEqual(1, counter);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItUsesValueFromDamageInterceptors()
    {
        var gameObject = new GameObject();

        var comp = gameObject.AddComponent<HealthController>();

        int damageAfterIntercept = 33;

        int damageReceived = 0;

        comp.OnDamage += x => damageReceived = x.OriginalChange;

        comp.OnBeforeDamage += x => new HealthChangeInterceptedEventArgs(x, damageAfterIntercept);

        comp.Damage(1);

        Assert.AreEqual(damageAfterIntercept, damageReceived);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItUsesValueFromHealInterceptors()
    {
        var gameObject = new GameObject();

        var comp = gameObject.AddComponent<HealthController>();

        int counter = 0;

        int healAfterIntercept = 33;

        int healingReceived = 0;

        comp.OnHeal += x =>
        {
            healingReceived = x.OriginalChange;
        };

        comp.OnBeforeHeal += x => new HealthChangeInterceptedEventArgs(x, healAfterIntercept);

        comp.Heal(1);

        Assert.AreEqual(healAfterIntercept, healingReceived);

        yield return null;
    }
}
