using DataContainerApp.Models.Profile;
using DataContainerApp.Models.Request;
using DataContainerApp.Models.Response;
using DataContainerApp.Storage;

namespace DataContainerApp.Services;

public class ResponseBuilder
{
    public DataResponse? Build(DataRequest request, ConsentProfile profile, UserData userData)
    {
        var response = new DataResponse
        {
            ResponseId = $"resp-{Guid.NewGuid()}",
            RequestId = request.RequestId,
            ProvidedFields = new List<ProvidedField>()
        };

        foreach (var field in request.RequestedFields)
        {
            // User hat das Feld nicht
            if (!userData.Data.TryGetValue(field.DataId, out var value))
                continue;

            var allowedPurposes = new List<PurposeReference>();

            foreach (var purpose in field.Purposes)
            {
                if (!IsGranted(profile, purpose.Pid, purpose.Fid))
                    continue;

                allowedPurposes.Add(new PurposeReference
                {
                    Pid = purpose.Pid,
                    Fid = purpose.Fid
                });
            }

            // kein Zweck erlaubt -> Feld nicht liefern
            if (allowedPurposes.Count == 0)
                continue;

            response.ProvidedFields.Add(new ProvidedField
            {
                DataId = field.DataId,
                Value = value,
                Purposes = allowedPurposes
            });
        }

        return response.ProvidedFields.Count > 0 ? response : null;
    }

    // Hilfsmethode
    private static bool IsGranted(
        ConsentProfile profile,
        string pid,
        string fid)
    {
        return profile.Grants.Any(g =>
            (g.Pid == pid || g.Pid == "*") &&
            (g.Fid == fid || g.Fid == "*") &&
            g.Granted
        );
    }
}
