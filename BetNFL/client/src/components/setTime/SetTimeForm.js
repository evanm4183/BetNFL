import { useState, useEffect } from "react";
import { Form, FormGroup, Label, Input, Button } from "reactstrap";
import "../../styles/form-styles.css";
import { getSiteTime } from "../../modules/siteTimeManager";

export default function SetTimeForm() {
    const [currentTime, setCurrentTime] = useState({week: "", year: ""});
    const [newTime, setNewTime] = useState({week: "", year: ""});

    const handleSubmit = () => {
        if (newTime.week === currentTime.week && newTime.year === currentTime.year) {
            window.alert("Site time remains unchanged");
            return;
        }
        
    }

    useEffect(() => {
        getSiteTime().then((siteTime) => {
            setCurrentTime({
                week: siteTime.currentWeek,
                year: siteTime.currentYear
            });
            setNewTime({
                week: siteTime.currentWeek,
                year: siteTime.currentYear
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
                    defaultValue={currentTime.week}
                    onChange={(e) => {
                        const copy = {...newTime};
                        copy.week = parseInt(e.target.value);
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
                    defaultValue={currentTime.year}
                    onChange={(e) => {
                        const copy = {...newTime};
                        copy.year = parseInt(e.target.value);
                        setNewTime(copy);
                    }} 
                /> 
            </FormGroup>
            <Button onClick={handleSubmit}>Submit</Button>
        </Form>
    );
}