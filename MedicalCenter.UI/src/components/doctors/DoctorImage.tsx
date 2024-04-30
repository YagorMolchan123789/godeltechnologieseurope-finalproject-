import React, { useState, useEffect } from 'react';
import getPublicImageUrlByName from '../../utils/GetPublicImageUrl';

interface DoctorImageProps {
    imageNameWithType: string;
    style?: React.CSSProperties;
}

const DoctorImage = ({ imageNameWithType, style }: DoctorImageProps) => {
    const defaultImg = 'default.png';
    const [image, setImage] = useState(
        getPublicImageUrlByName(imageNameWithType || defaultImg)
    );

    useEffect(() => {
        setImage(getPublicImageUrlByName(imageNameWithType || defaultImg));
    }, [imageNameWithType]);

    return (
        <img
            src={image}
            alt="Doctor"
            style={style}
            onError={() => setImage(getPublicImageUrlByName(defaultImg))}
        />
    );
};

export default DoctorImage;
