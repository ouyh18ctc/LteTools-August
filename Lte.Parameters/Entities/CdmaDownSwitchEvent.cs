using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lte.Parameters.Entities
{
    public class CdmaDownSwitchEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime RecordTime { get; set; }

        public short SrcBscId { get; set; }

        public int SrcCellId { get; set; }

        public byte SrcSectorId { get; set; }

        public int SrcFrequency { get; set; }

        public short DstBscId { get; set; }

        public int DstCellId { get; set; }

        public byte DstSectorId { get; set; }

        public int DstFrequency { get; set; }

        public string Imsi { get; set; }

        public string Meid { get; set; }

        public DateTime SrcReleaseTime { get; set; }

        public DateTime DstConnectTime { get; set; }

        public int SrcReleaseCauseId { get; set; }

        public double SrcStrength { get; set; }

        public double DstStrength { get; set; }

        public short SrcDelay { get; set; }

        public double SrcSlotBusy { get; set; }

        public double MainRssi { get; set; }

        public double SubRssi { get; set; }

        public double MainRot { get; set; }

        public double SubRot { get; set; }

        public int DownswitchTimes { get; set; }

        public int DownswitchCause { get; set; }

        public int DownswitchDuration { get; set; }
    }
}
