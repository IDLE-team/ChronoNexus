using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData : MonoBehaviour
{
    public static UpgradeData Instance;

    [SerializeField] private int _maxHPUpgradeValue;
    [SerializeField] private int _invincibleTimeUpgradeValue;

    [SerializeField] private int _hpDropAmountUpgradeValue;

    [SerializeField] private int _firearmDamageUpgradeValue;

    [SerializeField] private int _finisherMinHealthPercentUpgradeValue;

    [SerializeField] private int _slowTimePercentAfterFinisherUpgradeValue;

    [SerializeField] private int _timeStopDurationUpgradeValue;
    [SerializeField] private int _speedBonusUpgradeValue;
    [SerializeField] private int _inversionDurationUpgradeValue;

    private string MaxHPUpgrade = "MaxHPUpgrade";
    private string InvincibleTimeUpgrade = "InvincibleTimeUpgrade";
    private string HPDropAmount = "HPDropAmount";
    private string FirearmDamageUpgrade = "FirearmDamageUpgrade";
    private string FinisherMinHealthUpgrade = "FinisherMinHealthUpgrade";
    private string SlowTimeUpgrade = "SlowTimeUpgrade";
    private string TimeStopUpgrade = "TimeStopUpgrade";
    private string GlideUpgrade = "GlideUpgrade";
    private string InversionUpgrade = "InversionUpgrade";
    public int MaxHPUpgradeValue => _maxHPUpgradeValue;
    public int InvincibleTimeUpgradeValue => _invincibleTimeUpgradeValue;
    public int HpDropAmountUpgradeValue => _hpDropAmountUpgradeValue;

    public int FirearmDamageUpgradeValue => _firearmDamageUpgradeValue;
    public int FinisherMinHealthPercentUpgradeValue => _finisherMinHealthPercentUpgradeValue;
    public int SlowTimePercentAfterFinisherUpgradeValue => _slowTimePercentAfterFinisherUpgradeValue;

    public int TimeStopDurationUpgradeValue => _timeStopDurationUpgradeValue;
    public int SpeedBonusUpgradeValue => _speedBonusUpgradeValue;
    public int InversionDurationUpgradeValue => _inversionDurationUpgradeValue;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeStats();
    }

    private void InitializeStats()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            switch (type)
            {
                case (UpgradeType.Shell):
                    _maxHPUpgradeValue = PlayerPrefs.GetInt(MaxHPUpgrade, 0);
                    break;

                case (UpgradeType.Toughness):
                    _invincibleTimeUpgradeValue = PlayerPrefs.GetInt(InvincibleTimeUpgrade, 0);
                    break;

                case (UpgradeType.Pinata):
                    _hpDropAmountUpgradeValue = PlayerPrefs.GetInt(HPDropAmount, 0);
                    break;

                case (UpgradeType.Shooter):
                    _firearmDamageUpgradeValue = PlayerPrefs.GetInt(FirearmDamageUpgrade, 0);
                    break;

                case (UpgradeType.Executor):
                    _finisherMinHealthPercentUpgradeValue = PlayerPrefs.GetInt(FinisherMinHealthUpgrade, 0);
                    break;

                case (UpgradeType.Rage):
                    _slowTimePercentAfterFinisherUpgradeValue = PlayerPrefs.GetInt(SlowTimeUpgrade, 0);
                    break;

                case (UpgradeType.TimeStop):
                    _timeStopDurationUpgradeValue = PlayerPrefs.GetInt(TimeStopUpgrade, 0);
                    break;

                case (UpgradeType.Glide):
                    _speedBonusUpgradeValue = PlayerPrefs.GetInt(GlideUpgrade, 0);
                    break;

                case (UpgradeType.Inversion):
                    _inversionDurationUpgradeValue = PlayerPrefs.GetInt(InversionUpgrade, 0);
                    break;
            }
        }
    }

    public void SetStat(UpgradeType type, int value)
    {
        switch (type)
        {
            case (UpgradeType.Shell):
                _maxHPUpgradeValue = value;
                PlayerPrefs.SetInt(MaxHPUpgrade, _maxHPUpgradeValue);
                break;

            case (UpgradeType.Toughness):
                _invincibleTimeUpgradeValue = value;
                PlayerPrefs.SetInt(InvincibleTimeUpgrade, _invincibleTimeUpgradeValue);
                break;

            case (UpgradeType.Pinata):
                _hpDropAmountUpgradeValue = value;
                PlayerPrefs.SetInt(HPDropAmount, _hpDropAmountUpgradeValue);

                break;

            case (UpgradeType.Shooter):
                _firearmDamageUpgradeValue = value;
                PlayerPrefs.SetInt(FirearmDamageUpgrade, _firearmDamageUpgradeValue);

                break;

            case (UpgradeType.Executor):
                _finisherMinHealthPercentUpgradeValue = value;
                PlayerPrefs.SetInt(FinisherMinHealthUpgrade, _finisherMinHealthPercentUpgradeValue);

                break;

            case (UpgradeType.Rage):
                _slowTimePercentAfterFinisherUpgradeValue = value;
                PlayerPrefs.SetInt(SlowTimeUpgrade, _slowTimePercentAfterFinisherUpgradeValue);

                break;

            case (UpgradeType.TimeStop):
                _timeStopDurationUpgradeValue = value;
                PlayerPrefs.SetInt(TimeStopUpgrade, _timeStopDurationUpgradeValue);
                break;

            case (UpgradeType.Glide):
                _speedBonusUpgradeValue = value;
                PlayerPrefs.SetInt(GlideUpgrade, _speedBonusUpgradeValue);

                break;

            case (UpgradeType.Inversion):
                _inversionDurationUpgradeValue = value;
                PlayerPrefs.SetInt(InversionUpgrade, _inversionDurationUpgradeValue);
                break;
        }
    }
}

public enum UpgradeType
{
    Shell,
    Toughness,
    Pinata,
    Shooter,
    Executor,
    Rage,
    TimeStop,
    Glide,
    Inversion
}