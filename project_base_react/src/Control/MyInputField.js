import { Row, Col, Input } from 'reactstrap';

const InputField = ({ type = 'text', placeholder = '', label, ...props }) => {
    return (
        <Row className='p-1'>
            <Col sm={label ? 4 : ""}>
                {label} :
            </Col>
            <Col sm={label ? 8 : 12}>
                <Input
                    type={type}
                    placeholder={placeholder}
                >
                </Input>
            </Col>
        </Row>

    );
};

export default InputField;
