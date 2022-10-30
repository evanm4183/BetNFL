import { useState, useEffect } from "react";
import { Form, FormGroup, Input, Button, Label } from "reactstrap";
import { getCurrentUser, addFunds } from "../../modules/userProfileManager";

export default function ProfilePage() {
    const [currentUser, setCurrentUser] = useState();
    const [userObjForPost, setUserObjForPost] = useState();

    const getAndSetCurrentUser = () => {
        getCurrentUser().then((user) => {
            setCurrentUser(user);
            setUserObjForPost(user);
        });
    }

    useEffect(() => {
        getAndSetCurrentUser();
    }, []);

    return (
        <>
            <h2 className="form-title">Your Account</h2>
            <div className="form-container">
                <h4 className="form-title">Account Details</h4>
                <div className="form-section">
                    <div><strong>Username:</strong> {currentUser?.username}</div>
                    <div><strong>Email:</strong> {currentUser?.email}</div>
                    <div><strong>Account Type:</strong> {currentUser?.userType.name.toUpperCase()}</div>
                </div>
                <h4 className="form-title">Available Funds</h4>
                <div className="form-section">
                    <h4 style={{display: "flex", justifyContent: "center", fontWeight: "normal"}}>
                        ${currentUser?.availableFunds.toFixed(2)}
                    </h4>
                </div>
                <h4 className="form-title">Add Funds</h4>
                <div className="form-section">
                    <Form>
                        <FormGroup>
                            <Label for="addFunds">Enter an Amount</Label>
                            <Input 
                                type="number" 
                                name="addFunds" 
                                placeholder="$0.00"
                                onChange={(e) => {
                                    const copy = {...userObjForPost}
                                    copy.availableFunds = currentUser.availableFunds + parseFloat(e.target.value);
                                    setUserObjForPost(copy);
                                }}
                            />
                            <Button 
                                style={{marginTop: "10px"}}
                                onClick={() => {
                                    addFunds(userObjForPost).then(getAndSetCurrentUser);
                                }}
                            >
                                Process Transaction
                            </Button>
                        </FormGroup>
                    </Form>
                </div>
            </div>
        </>
    )
}