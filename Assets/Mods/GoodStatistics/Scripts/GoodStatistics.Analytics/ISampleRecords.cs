namespace GoodStatistics.Analytics {
  public interface ISampleRecords {

    bool WasAtFullOrZeroPreviously();

    int GetSamplesCount();

    float GetDayTimestampAt(int index);

    int GetTotalAmountAt(int index);

    int GetTotalCapacityAt(int index);

    int GetMaxCapacity();

  }
}