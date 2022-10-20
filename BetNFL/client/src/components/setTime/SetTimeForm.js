import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from "reactstrap";
import "../../styles/form-styles.css";
import { getSiteTime, updateSiteTime } from "../../modules/siteTimeManager";

export default function SetTimeForm() {
    const [currentTime, setCurrentTime] = useState({"currentWeek": "", "currentYear": ""});
    const [newTime, setNewTime] = useState({"currentWeek": "", "currentYear": ""});
    const navigate = useNavigate();

    const handleSubmit = () => {
        if (newTime.currentWeek === currentTime.currentWeek 
            && newTime.currentYear === currentTime.currentYear) {
            window.alert("Site time remains unchanged");
            return;
        }

        updateSiteTime(newTime).then(() => {navigate("/")});
    }

    useEffect(() => {
        getSiteTime().then((siteTime) => {
            setCurrentTime({
                currentWeek: siteTime.currentWeek,
                currentYear: siteTime.currentYear
            });
            setNewTime({
                currentWeek: siteTime.currentWeek,
                currentYear: siteTime.currentYear
            });
        })
    }, []);

    return (
        <Form className="form-container" style={{width: "25%"}}>
            <FormGroup>
                <Label for="exampleEmail">Current Week</Label>
                <Input 
                    type="number" 
                    name="currentWeek" 
                    id="current-week" 
                    defaultValue={currentTime.currentWeek}
                    onChange={(e) => {
                        const copy = {...newTime};
                        copy.currentWeek = parseInt(e.target.value);
                        setNewTime(copy);
                    }}
                /> 
            </FormGroup>
            <FormGroup >
                <Label for="exampleEmail">Current Year</Label>
                <Input 
                    type="number" 
                    name="currentYear" 
                    id="current-year" 
                    defaultValue={currentTime.currentYear}
                    onChange={(e) => {
                        const copy = {...newTime};
                        copy.currentYear = parseInt(e.target.value);
                        setNewTime(copy);
                    }} 
                /> 
            </FormGroup>
            <Button onClick={handleSubmit}>Submit</Button>
        </Form>
    );
}