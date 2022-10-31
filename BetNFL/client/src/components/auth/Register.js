import React, { useState, useEffect } from "react";
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import { useNavigate } from "react-router-dom";
import { register } from "../../modules/authManager";
import { getPublicUserTypes } from "../../modules/userTypeManager";

export default function Register() {
  const navigate = useNavigate();

  const [username, setUsername] = useState();
  const [email, setEmail] = useState();
  const [userType, setUserType] = useState();
  const [password, setPassword] = useState();
  const [confirmPassword, setConfirmPassword] = useState();
  const [userTypes, setUserTypes] = useState();

  const registerClick = (e) => {
    e.preventDefault();
    if (password && password !== confirmPassword) {
      alert("Passwords don't match. Do better.");
    } else {
      const userProfile = {
        username,
        userType: {id: userType},
        email,
      };
      register(userProfile, password).then(() => navigate("/"));
    }
  };

  useEffect(() => {
    getPublicUserTypes().then((userTypes) => {
      setUserTypes(userTypes);
    });
  }, []);

  return (
    <Form onSubmit={registerClick}>
      <fieldset>
        <FormGroup>
          <Label htmlFor="username">Username</Label>
          <Input
            id="username"
            type="text"
            onChange={(e) => setUsername(e.target.value)}
          />
        </FormGroup>
        <FormGroup>
          <Label for="email">Email</Label>
          <Input
            id="email"
            type="text"
            onChange={(e) => setEmail(e.target.value)}
          />
        </FormGroup>
        <FormGroup>
        <Label for="userType">Account Type</Label>
          <Input 
            type="select" 
            name="userType" 
            id="user-type" 
            onChange={(e) => {setUserType(parseInt(e.target.value))}}
          >
            <option id="team--0">Select a Type...</option>
            {
                userTypes?.map((type) => {
                    return <option 
                        key={`type--${type.id}`}
                        value={type.id}
                    >{type.name}</option>
                })
            }
          </Input>
        </FormGroup>
        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            id="password"
            type="password"
            onChange={(e) => setPassword(e.target.value)}
          />
        </FormGroup>
        <FormGroup>
          <Label for="confirmPassword">Confirm Password</Label>
          <Input
            id="confirmPassword"
            type="password"
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </FormGroup>
        <FormGroup>
          <Button>Register</Button>
        </FormGroup>
      </fieldset>
    </Form>
  );
}
