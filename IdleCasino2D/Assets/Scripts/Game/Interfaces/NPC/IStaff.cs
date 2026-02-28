using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff : INpc
{
    public StaffType StaffType { get; }

    public void Initialize();
    public void Dispose();
}

public interface IStaffModel { }
public interface IStaffView { }
