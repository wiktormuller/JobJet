import React, { useState } from 'react';
import UserService from '../services/UserService'

function ViewUserComponent(props)
{
    const[userState, setUserState] = useState({
        id: this.props.match.params.id,
        user: {}
    });

    // Similar to componentDidMount and componentDidUpdate
    useEffect(() => {
        UserService.getUserById(userState.id).then((res) => {
            setUserState({
                user: res.data
            })
        });
    });

    return (
        <div>
            <br></br>
            <div className = "card col-md-6 offset-md-3">
                <h3 className = "text-center"> View User Details</h3>
                <div className = "card-body">
                    <div className = "row">
                        <label>Id:</label>
                        <div> { userState.user.id }</div>
                    </div>
                    <div className = "row">
                        <label>Username:</label>
                        <div> { userState.user.userName }</div>
                    </div>
                    <div className = "row">
                        <label>Email:</label>
                        <div> { userState.user.email }</div>
                    </div>
                </div>

            </div>
        </div>
    );
}

export default ViewUserComponent;