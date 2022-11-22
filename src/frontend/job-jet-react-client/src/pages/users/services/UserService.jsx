import axios from 'axios';
import { AuthenticationContext } from "../../../common/AuthenticationContext";

const USERS_API_BASE_URL = "https://jobjet.azurewebsites.net/api/v1/users";

class UserService
{
    getUsers(accessToken)
    {
        return axios.get(
            USERS_API_BASE_URL,
            {
                headers: {
                  "Authorization": "Bearer " + accessToken
                }
            }
        );
    }

    getUserById(usersId){
        return axios.get(USERS_API_BASE_URL + '/' + usersId);
    }

    updateUser(user, userId){
        return axios.put(USERS_API_BASE_URL + '/' + userId, user);
    }

    deleteUser(userId){
        return axios.delete(USERS_API_BASE_URL + '/' + userId);
    }
}

export default new UserService()