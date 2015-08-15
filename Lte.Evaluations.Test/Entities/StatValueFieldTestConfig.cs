using System.Collections.Generic;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Test.Entities
{
    public class StatValueFieldTestConfig
    {
        protected StatValueFieldRepository Repository;
        protected const string resultString = @"<Setting>
  <Field ID=""field1"">
    <Interval>
      <LowLevel>0</LowLevel>
      <UpLevel>1</UpLevel>
      <A>255</A>
      <B>255</B>
      <R>255</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>1</LowLevel>
      <UpLevel>2</UpLevel>
      <A>5</A>
      <B>255</B>
      <R>255</R>
      <G>5</G>
    </Interval>
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>255</A>
      <B>25</B>
      <R>25</R>
      <G>255</G>
    </Interval>
  </Field>
  <Field ID=""field2"">
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>155</A>
      <B>155</B>
      <R>5</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>4</LowLevel>
      <UpLevel>5</UpLevel>
      <A>5</A>
      <B>35</B>
      <R>35</R>
      <G>5</G>
    </Interval>
  </Field>
</Setting>";

        protected List<StatValueField> statValueFieldList;

        protected void Initialize()
        {
            statValueFieldList = new List<StatValueField>
            {
                new StatValueField
                {
                    FieldName = "field1",
                    IntervalList = new List<StatValueInterval>
                    {
                        new StatValueInterval
                        {
                            IntervalLowLevel = 0,
                            IntervalUpLevel = 1,
                            Color = new Color
                            {
                                ColorA = 255,
                                ColorB = 255,
                                ColorG = 255,
                                ColorR = 255
                            }
                        },
                        new StatValueInterval
                        {
                            IntervalLowLevel = 1,
                            IntervalUpLevel = 2,
                            Color = new Color
                            {
                                ColorA = 5,
                                ColorB = 255,
                                ColorG = 5,
                                ColorR = 255
                            }
                        },
                        new StatValueInterval
                        {
                            IntervalLowLevel = 2,
                            IntervalUpLevel = 3,
                            Color = new Color
                            {
                                ColorA = 255,
                                ColorB = 25,
                                ColorG = 255,
                                ColorR = 25
                            }
                        }
                    }
                },
                new StatValueField
                {
                    FieldName = "field2",
                    IntervalList = new List<StatValueInterval>
                    {
                        new StatValueInterval
                        {
                            IntervalLowLevel = 2,
                            IntervalUpLevel = 3,
                            Color = new Color
                            {
                                ColorA = 155,
                                ColorB = 155,
                                ColorG = 255,
                                ColorR = 5
                            }
                        },
                        new StatValueInterval
                        {
                            IntervalLowLevel = 4,
                            IntervalUpLevel = 5,
                            Color = new Color
                            {
                                ColorA = 5,
                                ColorB = 35,
                                ColorG = 5,
                                ColorR = 35
                            }
                        }
                    }
                }
            };
        }

    }
}