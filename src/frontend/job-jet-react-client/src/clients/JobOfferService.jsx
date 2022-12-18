import axios from 'axios';

const JOB_OFFERS_API_BASE_URL = "https://jobjet.azurewebsites.net/api/v1/job-offers";

class JobOfferService {

    getJobOffers(searchText, selectedSeniorityLevelId, selectedWorkSpecification, selectedEmploymentTypeId, selectedTechnologyTypesId){
        var query = new URLSearchParams();

        if (searchText !== undefined && searchText !== null && searchText !== '')
        {
            query.append("GeneralSearchByText", searchText);
        }
        
        if (selectedSeniorityLevelId !== undefined && selectedSeniorityLevelId !== 0)
        {
            query.append("SeniorityLevelId", selectedSeniorityLevelId);
        }

        if (selectedWorkSpecification !== undefined && selectedWorkSpecification !== '')
        {
            query.append("WorkSpecification", selectedWorkSpecification);
        }

        if (selectedEmploymentTypeId !== undefined && selectedEmploymentTypeId !== 0)
        {
            query.append("EmploymentTypeId", selectedEmploymentTypeId);
        }

        if (selectedTechnologyTypesId !== undefined && selectedTechnologyTypesId > 0)
        {
            query.append("TechnologyIds", selectedTechnologyTypesId);
        }

        var resultUrl = JOB_OFFERS_API_BASE_URL;
        if (query.toString() !== undefined && query.toString() !== '')
        {
            resultUrl = resultUrl + '?' + query.toString();
        }

        return axios.get(resultUrl);
    }

    createJobOffer(jobOffer){
        return axios.post(JOB_OFFERS_API_BASE_URL, jobOffer);
    }

    getJobOfferById(jobOfferId){
        return axios.get(JOB_OFFERS_API_BASE_URL + '/' + jobOfferId);
    }

    updateJobOffer(jobOffer, jobOfferId){
        return axios.put(JOB_OFFERS_API_BASE_URL + '/' + jobOfferId, jobOffer);
    }

    deleteJobOffer(jobOfferId){
        return axios.delete(JOB_OFFERS_API_BASE_URL + '/' + jobOfferId);
    }

    getJobOfferApplications(jobOfferId)
    {
        let config = {
            headers: {
                'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6IkNFTyIsImp0aSI6IjRkN2ZmNWQ5LWZhOWEtNGQzNi04N2M4LTI4NzAwM2FjODYzNiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMSIsImVtYWlsIjoiY2VvQGpvYmpldC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiQWRtaW5pc3RyYXRvciIsIlVzZXIiXSwiZXhwIjoxNjcxNDAyOTYyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAzIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMyJ9.3C2JE8KdmGa9ri6Tjclr09LxL3y3vhr1OhYtcnqLeqk'
            }
        }

        return axios.get(JOB_OFFERS_API_BASE_URL + '/' + jobOfferId + '/' + 'offer-applications', config);
    }

    getJobOfferApplicationFile(jobOfferId, jobOfferApplicationId)
    {
        let config = {
            headers: {
                'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6IkNFTyIsImp0aSI6IjRkN2ZmNWQ5LWZhOWEtNGQzNi04N2M4LTI4NzAwM2FjODYzNiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMSIsImVtYWlsIjoiY2VvQGpvYmpldC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiQWRtaW5pc3RyYXRvciIsIlVzZXIiXSwiZXhwIjoxNjcxNDAyOTYyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAzIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMyJ9.3C2JE8KdmGa9ri6Tjclr09LxL3y3vhr1OhYtcnqLeqk',
                'Content-Type': 'application/octet-stream'
            },
            responseType: 'blob'
        }

        return axios.get(JOB_OFFERS_API_BASE_URL + '/' + jobOfferId + '/' + 'offer-applications' + '/' + jobOfferApplicationId, config);
    }

    sendJobOfferApplication(jobOfferId, userEmail, phoneNumber, file)
    {
        var formData = new FormData();
        formData.append("File", file)
        formData.append('UserEmail', userEmail);
        formData.append('PhoneNumber', phoneNumber);

        return axios.post(JOB_OFFERS_API_BASE_URL + '/' + jobOfferId + '/' + 'offer-applications', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
              },
        });
    }
}

export default new JobOfferService();