using CohesiveWizardry.Storage.Dtos.Requests;

namespace CohesiveWizardry.Storage.Dtos.Database
{
    internal interface IGetStorageDto : IStorageDto
    {
        public string Id { get; set; }
        public DateTimeOffset LastModifiedAtUtc { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public long Revision { get; set; }
    }
}
