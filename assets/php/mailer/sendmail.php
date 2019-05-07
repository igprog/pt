<?php

require("class.phpmailer.php");
error_reporting(E_ALL);
ini_set('display_errors', '1');

$data = file_get_contents("php://input");
$postData = json_decode($data);
$sendto = $postData->sendto;
$subject = $postData->subject;
$body = $postData->body;
$cc = $postData->cc;

$mail = new PHPMailer();
$mail->CharSet = "UTF-8";
$mail->IsSMTP();
$mail->Host = "mail.promo-tekstil.com";
$mail->SMTPAuth = true;
$mail->SMTPSecure = "ssl";
$mail->Port = 465;
$mail->Username = "info@promo-tekstil.com";
$mail->Password = "PromoTekstil$";

$mail->From = "info@promo-tekstil.com";
$mail->FromName = "Promo-Tekstil.com";
$mail->AddAddress($sendto);
foreach ($cc as $m) {
  $mail->AddCC($m);
}
//$mail->AddReplyTo("mail@mail.com");

$mail->IsHTML(true);

$mail->Subject = $subject;
$mail->Body = $body;
//$mail->AltBody = "This is the body in plain text for non-HTML mail clients";

if(!$mail->Send())
{
echo "Mail nije bilo moguće poslati.<p>";
echo "Mailer Error: " . $mail->ErrorInfo;
exit;
}

echo "mail sent successfully";

?>